using CACS.Framework.Mvc.Controllers;
using CACS.Framework.Mvc.Filters;
using CACS.Framework.Mvc.Models;
using CACSLibrary;
using CACSLibrary.Data;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.Interface;
using HT.Plugin.ProgramPublish.WebSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Controllers
{
    public class NotificationController : CACSController
    {
        IRepository<Notification> _notificationRepository;
        IRepository<Group> _groupRepository;
        IRepository<Terminal> _terminalRepository;
        ITerminalService _terminalService;

        public NotificationController(
            IRepository<Notification> notificationRepository,
            IRepository<Group> groupRepository,
            IRepository<Terminal> terminalRepository,
            ITerminalService terminalService)
        {
            _notificationRepository = notificationRepository;
            _groupRepository = groupRepository;
            _terminalRepository = terminalRepository;
            _terminalService = terminalService;
        }

        [AccountTicket]
        public ActionResult List(ListModel model)
        {
            var query = _notificationRepository.Table;
            query = !string.IsNullOrEmpty(model.Search) ? query.Where(e => e.Title.Contains(model.Search) || e.Content.Contains(model.Search)) : query;

            if (model.Sort.Count <= 0)
                query = query.OrderByDescending(e => e.StartTime);
            else
                model.Sort.ForEach(sort =>
                    query = QueryBuilder.DataSorting(query, sort.Key, sort.Value.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? true : false));

            var result = new PagedList<Notification>(query, model.Page - 1, model.Limit)
                ?? new PagedList<Notification>(new List<Notification>(), model.Page - 1, model.Limit);

            return JsonList(result.Select(e => (Notification)e.Clone()).ToArray(), result.TotalCount);
        }

        [AccountTicket]
        public ActionResult Load(int id)
        {
            var domain = _notificationRepository.GetById(id);
            return Json((Notification)domain.Clone());
        }

        [AccountTicket(AuthorizeName = "编辑", Group = "消息管理")]
        public ActionResult Save(Notification model)
        {
            var domain = _notificationRepository.GetById(model.Id);
            if (domain == null)
            {
                _notificationRepository.Insert(model);
            }
            else
            {
                domain.Content = model.Content;
                domain.EndTime = model.EndTime;
                domain.StartTime = model.StartTime;
                domain.Title = model.Title;
                _notificationRepository.Update(domain);
            }
            return Json(model.Id);
        }

        [AccountTicket(AuthorizeName = "删除", Group = "消息管理")]
        public ActionResult Delete(int id)
        {
            var domain = _notificationRepository.GetById(id);
            var terminals = _terminalRepository.Table.Where(e => e.NotificationId == id).ToList();
            if (terminals.Count > 0)
            {
                terminals.ForEach(e => e.NotificationId = null);
                _terminalRepository.Update(terminals.ToArray());
            }
            _notificationRepository.Delete(domain);
            return Json(true);
        }


        [AccountTicket(AuthorizeName = "发布", Group = "消息管理")]
        public ActionResult PublishGroups(NotificationPublishModel model)
        {
            var terminals = _terminalService.GetTerminalByGroups(model.Publishs.ToArray());
            terminals.ForEach(e => e.NotificationId = model.NotificationId);
            _terminalRepository.Update(terminals.ToArray());
            return Json(true);
        }

        [AccountTicket(AuthorizeId = "/ProgramPublish/Notification/PublishGroups")]
        public ActionResult PublishTerminals(NotificationPublishModel model)
        {
            var terminals = _terminalRepository.Table.Where(e => model.Publishs.Contains(e.Id)).ToList();
            terminals.ForEach(e => e.NotificationId = model.NotificationId);
            _terminalRepository.Update(terminals.ToArray());
            return Json(true);
        }
    }
}