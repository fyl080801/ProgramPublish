using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class NotificationPublishModel
    {
        public int NotificationId { get; set; }

        public List<int> Publishs { get; set; } = new List<int>();
    }
}