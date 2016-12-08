using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class ControlModel : BaseEntityModel<int>
    {
        public string Command { get; set; }

        public string Args { get; set; }
    }
}