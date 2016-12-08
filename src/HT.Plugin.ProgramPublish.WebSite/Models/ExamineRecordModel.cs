using CACS.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HT.Plugin.ProgramPublish.WebSite.Models
{
    public class ExamineRecordModel : BaseEntityModel<int>
    {
        public int ProgramId { get; set; }// int FALSE   素材Id

        public int UserId { get; set; }//      int FALSE   审核人Id

        public DateTime ExamineTime { get; set; }//     datetime FALSE   审核时间

        public string State { get; set; }

        public string Remark { get; set; }//      nvarchar max         TRUE 描述

        public string UserName { get; set; }
    }
}