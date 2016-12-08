using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class ExamineModel : BaseModel
    {
        public int Id { get; set; }

        public int State { get; set; }

        public string Comment { get; set; }
    }
}