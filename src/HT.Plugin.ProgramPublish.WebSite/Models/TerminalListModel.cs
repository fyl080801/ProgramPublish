using CACS.Framework.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class TerminalListModel : ListModel
    {
        public int? GroupId { get; set; } = null;
    }
}