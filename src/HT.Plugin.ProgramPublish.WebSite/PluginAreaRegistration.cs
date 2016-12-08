using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HT.Plugin.ProgramPublish.WebSite
{
    public class PluginAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "ProgramPublish"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HT.Plugin.ProgramPublish.WebSite",
                "ProgramPublish/{controller}/{action}/{id}",
                new { action = "List", id = UrlParameter.Optional },
                new[] { "HT.Plugin.ProgramPublish.WebSite.Controllers" });
        }
    }
}