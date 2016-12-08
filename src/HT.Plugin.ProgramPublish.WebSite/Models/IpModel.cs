using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class IpModel : BaseEntityModel<int>
    {
        public string Ip { get; set; }

        public string Sub { get; set; }

        public string Gate { get; set; }

        public string Dns { get; set; }
    }
}