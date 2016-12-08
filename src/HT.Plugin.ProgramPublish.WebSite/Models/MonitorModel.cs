using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class MonitorModel : BaseEntityModel<int>
    {
        public int State { get; set; }

        public string Cpu { get; set; }

        public string ComputerName { get; set; }

        public string Ip { get; set; }

        public string Mac { get; set; }

        public string Memory { get; set; }

        public string DiskTotal { get; set; }

        public string DiskUse { get; set; }

        public string DiskFree { get; set; }
    }
}