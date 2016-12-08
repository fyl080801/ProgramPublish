using CACS.Framework.Mvc;
using HT.Plugin.ProgramPublish.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class TerminalModel : BaseEntityModel<int>
    {
        public string Name { get; set; }

        public string TerminalCode { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public TerminalTypes TerminalType { get; set; }

        public string IpAddress { get; set; }

        public static TerminalModel ToModel(Terminal domain)
        {
            return new TerminalModel()
            {
                GroupId = domain.GroupId,
                GroupName = domain.Group.Name,
                Id = domain.Id,
                IpAddress = domain.IpAddress,
                Name = domain.Name,
                TerminalCode = domain.TerminalCode,
                TerminalType = domain.TerminalType
            };
        }
    }
}