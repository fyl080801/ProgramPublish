using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class UploadModel : BaseModel
    {
        public string Ruid { get; set; }

        public string Part { get; set; }

        public string Extension { get; set; }

        public string FileName { get; set; }
    }
}