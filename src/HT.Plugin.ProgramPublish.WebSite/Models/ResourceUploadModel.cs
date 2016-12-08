using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class ResourceUploadModel : BaseModel
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Ext { get; set; }

        public int Size { get; set; }

        public string Ruid { get; set; }
    }
}