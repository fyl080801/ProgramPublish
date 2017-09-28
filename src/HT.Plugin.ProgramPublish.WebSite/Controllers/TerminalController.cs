﻿using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACSLibrary;
using CACSLibrary.Data;
using CACSLibrary.Profile;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.Profiles;
using HT.Plugin.ProgramPublish.WebSite.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class TerminalController : CACSController
    {
        IRepository<Program> _programRepository;
        IRepository<Terminal> _terminalRepository;
        IRepository<Group> _groupRepository;
        IRepository<GroupUser> _groupuserRepository;
        IProfileManager _profileManager;

        public TerminalController(
            IRepository<Program> programRepository,
            IRepository<Terminal> terminalRepository,
            IRepository<Group> groupRepository,
            IRepository<GroupUser> groupuserRepository,
            IProfileManager profileManager)
        {
            _programRepository = programRepository;
            _terminalRepository = terminalRepository;
            _groupRepository = groupRepository;
            _groupuserRepository = groupuserRepository;
            _profileManager = profileManager;
        }

        [AccountTicket]
        public ActionResult List(TerminalListModel model)
        {
            var query = _terminalRepository.Table;
            if (model.GroupId.HasValue)
            {
                var group = _groupRepository.GetById(model.GroupId.Value);
                var groups = _groupRepository.Table
                    .Where(e => e.RelationPath.Contains(group.RelationPath))
                    .Select(e => e.Id);
                query = query.Where(e => groups.Contains(e.GroupId));
            }
            var groupuser = _groupuserRepository.GetById(Convert.ToInt32(User.Identity.GetUserId()));
            query = groupuser != null ? query.Where(e => e.Group.RelationPath.Contains(groupuser.Group.RelationPath)) : query;
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Name.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderBy(e => e.Id);
            else
                model.Sort.ForEach(sort =>
                {
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false);
                });
            if (model.Page > 0 && model.Limit > 0)
            {
                var result = new PagedList<Terminal>(query, model.Page - 1, model.Limit)
                    ?? new PagedList<Terminal>(new List<Terminal>(), model.Page - 1, model.Limit);

                return JsonList(result.Select(e => (Terminal)e.Clone()).ToArray(), result.TotalCount);
            }
            else
            {
                return JsonList(query.ToArray().Select(e => (Terminal)e.Clone()).ToArray());
            }
        }

        [AccountTicket]
        public ActionResult Load(int id)
        {
            var domain = _terminalRepository.GetById(id);
            return Json(domain.Clone());
        }

        [AccountTicket(AuthorizeName = "编辑", Group = "终端管理")]
        public ActionResult Save(Terminal model)
        {
            var domain = _terminalRepository.GetById(model.Id);
            if (domain != null)
            {
                domain.GroupId = model.GroupId;
                domain.IpAddress = model.IpAddress;
                domain.MacAddress = model.MacAddress;
                domain.Name = model.Name;
                domain.TerminalCode = model.TerminalCode;
                domain.TerminalType = model.TerminalType;
                domain.UserName = model.UserName;
                domain.Password = model.Password;
                _terminalRepository.Update(domain);
            }
            else
            {
                _terminalRepository.Insert(model);
            }
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeName = "删除", Group = "终端管理")]
        public ActionResult Delete(int id)
        {
            var domain = _terminalRepository.GetById(id);
            _terminalRepository.Delete(domain);
            return Json(true);
        }

        /// <summary>
        /// 设定终端节目
        /// </summary>
        /// <param name="models"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [AccountTicket(AuthorizeName = "设置节目", Group = "终端管理")]
        public ActionResult SetPrograms(Program[] models, int id)
        {
            if (models == null) models = new Program[0];
            var selectedPrograms = models.Select(e => e.Id).ToArray();
            var domain = _terminalRepository.GetById(id);
            var programs = _programRepository.Table.Where(e => selectedPrograms.Contains(e.Id));
            domain.Programs.Clear();
            programs.ToList().ForEach(e => domain.Programs.Add(e));
            _terminalRepository.Update(domain);

            var profile = _profileManager.Get<ResourceProfile>();
            if (!Directory.Exists(profile.TerminalFlag)) Directory.CreateDirectory(profile.TerminalFlag);
            using (var sw = new StreamWriter(new FileStream(
                string.Format("{0}/{1}.txt", profile.TerminalFlag, domain.TerminalCode),
                FileMode.Create, FileAccess.ReadWrite)))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Close();
            }

            return Json(true);
        }

        /// <summary>
        /// 终端监控
        /// </summary>
        /// <param name="terminals"></param>
        /// <returns></returns>
        [AccountTicket]
        public ActionResult Monitor(int[] terminals)
        {
            var query = _terminalRepository.Table
                .Where(e => terminals.Contains(e.Id) && !string.IsNullOrEmpty(e.IpAddress))
                .ToArray();

            var monitors = new List<MonitorModel>();
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 10;
            options.CancellationToken = tokenSource.Token;

            Parallel.ForEach(query, options, (terminal, state) =>
            {
                var model = GetTerminalInfo(terminal.IpAddress, terminal.UserName, terminal.Password);
                model.Id = terminal.Id;
                model.Ip = terminal.IpAddress;
                model.Mac = terminal.MacAddress;
                monitors.Add(model);
            });

            return JsonList(monitors.ToArray());
        }

        /// <summary>
        /// 远程控制
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AccountTicket(AuthorizeName = "远程控制", Group = "终端管理")]
        public ActionResult Control(ControlModel model)
        {
            var domain = _terminalRepository.GetById(model.Id);
            if (model.Command == "Startup")
            {
                WakeUp(domain.IpAddress, domain.MacAddress);
            }
            else
            {
                InvokeMethod(domain.IpAddress, domain.UserName, domain.Password, model.Command);
            }
            return Json(true);
        }

        /// <summary>
        /// 修改IP
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AccountTicket(AuthorizeId = "/ProgramPublish/Terminal/Control")]
        public ActionResult ChangeIp(IpModel model)
        {
            var domain = _terminalRepository.GetById(model.Id);
            SetIPAddress(
                domain.IpAddress,
                domain.UserName,
                domain.Password,
                model.Ip,
                model.Sub);
            domain.IpAddress = model.Ip;
            _terminalRepository.Update(domain);
            return Json(model);
        }

        protected static void WakeUp(string ip, string mac)
        {
            var macBytes = mac.Split(':')
                .Select(e => Convert.ToByte("0x" + e, 16))
                .ToArray();
            byte[] packet = new byte[17 * 6];
            for (int i = 0; i < 6; i++)
                packet[i] = 0xFF;
            for (int i = 1; i <= 16; i++)
                for (int j = 0; j < 6; j++)
                    packet[i * 6 + j] = macBytes[j];
            new UdpClient()
                .Send(packet, packet.Length, new IPEndPoint(IPAddress.Broadcast, 9090));
        }

        protected static void InvokeMethod(string ip, string username, string password, string command)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Username = username;
            options.Password = password;
            options.Authority = "ntlmdomain:DOMAIN";
            options.EnablePrivileges = true;
            var mngPath = new ManagementPath(@"\\" + ip + @"\root\cimv2:Win32_Process");
            var scope = new ManagementScope(mngPath, options);
            try
            {
                scope.Connect();
                var classInstance = new ManagementClass(scope, mngPath, new ObjectGetOptions());
                var inParams = classInstance.GetMethodParameters("Create");
                inParams.SetPropertyValue("CommandLine", command);
                classInstance.InvokeMethod("Create", inParams, new InvokeMethodOptions());
            }
            catch (Exception ex)
            {
                throw new CACSException(ex.Message, ex);
            }
        }

        protected static void SetIPAddress(string orgIp, string username, string password, string newip, string newsubmask)
        {
            ConnectionOptions options = new ConnectionOptions();
            options.Username = username;
            options.Password = password;
            options.Authority = "ntlmdomain:DOMAIN";
            options.EnablePrivileges = true;
            var mngPath = new ManagementPath(@"\\" + orgIp + @"\root\cimv2");
            var scope = new ManagementScope(mngPath, options);
            try
            {
                scope.Connect();
                using (ManagementClass wmi = new ManagementClass(scope, new ManagementPath("Win32_NetworkAdapter"), new ObjectGetOptions()))
                {
                    ManagementObjectCollection moc = wmi.GetInstances();
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["NetConnectionStatus"] == null || mo["NetConnectionStatus"].ToString() != "2")
                            continue;

                        ManagementClass commandWmi = new ManagementClass(scope, new ManagementPath("Win32_Process"), new ObjectGetOptions());
                        var inParams = commandWmi.GetMethodParameters("Create");
                        inParams.SetPropertyValue("CommandLine", "netsh interface ip set address \"" + mo["NetConnectionID"].ToString() + "\" static " + newip + " " + newsubmask);
                        commandWmi.InvokeMethod("Create", inParams, new InvokeMethodOptions());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new CACSException(ex.Message, ex);
            }
        }

        protected static MonitorModel GetTerminalInfo(string ip, string username, string password)
        {
            var model = new MonitorModel();
            var isConnectioned = false;
            var options = new ConnectionOptions()
            {
                Username = username,
                Password = password,
                Authority = "ntlmdomain:DOMAIN",
                EnablePrivileges = true
            };
            var mngPath = new ManagementPath(@"\\" + ip + @"\root\cimv2");
            var scope = new ManagementScope(mngPath, options);
            try
            {
                //scope.Connect();

                var systemQuery = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                var systemSearcher = new ManagementObjectSearcher(scope, systemQuery);
                var systemCollection = systemSearcher.Get();
                foreach (ManagementObject returnValue in systemCollection)
                {
                    model.ComputerName = returnValue["csname"].ToString();
                    isConnectioned = returnValue.Scope.IsConnected;
                }

                var capacity = 0.0;
                var physicalMemoryClass = new ManagementClass(scope, new ManagementPath("Win32_PhysicalMemory"), new ObjectGetOptions());
                var pmInstances = physicalMemoryClass.GetInstances();
                foreach (ManagementObject pm in pmInstances)
                {
                    capacity += ((Math.Round(Int64.Parse(pm.Properties["Capacity"].Value.ToString()) / 1024 / 1024 / 1024.0, 1)));
                }

                var available = 0.0;
                var freeMemoryClass = new ManagementClass(scope, new ManagementPath("Win32_PerfFormattedData_PerfOS_Memory"), new ObjectGetOptions());
                var freeInstances = freeMemoryClass.GetInstances();
                foreach (ManagementObject pm in freeInstances)
                {
                    available += ((Math.Round(Int64.Parse(pm.Properties["AvailableMBytes"].Value.ToString()) / 1024.0, 1)));
                }

                model.Memory = (capacity - available).ToString() + " / " + capacity.ToString() + " G";

                using (var cpuClass = new ManagementClass(scope, new ManagementPath("Win32_PerfFormattedData_PerfOS_Processor"), new ObjectGetOptions()))
                {
                    var cpuInstances = cpuClass.GetInstances();
                    foreach (ManagementObject m in cpuInstances)
                    {
                        model.Cpu = m.Properties["PercentProcessorTime"].Value.ToString() + " %";
                    }
                }

                model.State = 1;

                return model;
            }
            catch (Exception ex)
            {
                if (isConnectioned)
                {
                    model.State = 1;
                    return model;
                }
                else
                    return new MonitorModel() { State = 0 };
            }
        }
    }
}