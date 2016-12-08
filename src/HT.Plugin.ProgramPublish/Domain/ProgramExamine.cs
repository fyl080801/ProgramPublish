using CACS.Framework.Domain;
using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain
{
    [Table("pub_ProgramExamine")]
    public class ProgramExamine : BaseEntity<int>, ICloneable
    {
        public int ProgramId { get; set; }// int FALSE   素材Id

        public int UserId { get; set; }//      int FALSE   审核人Id

        public DateTime ExamineTime { get; set; }//     datetime FALSE   审核时间

        [MaxLength(20)]
        public string State { get; set; }

        public string Remark { get; set; }//      nvarchar max         TRUE 描述

        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public object Clone()
        {
            return new ProgramExamine()
            {
                ExamineTime = this.ExamineTime,
                Id = this.Id,
                ProgramId = this.ProgramId,
                Remark = this.Remark,
                State = this.State,
                UserId = this.UserId,
                User = new User()
                {
                    UserName = this.User.UserName,
                    FirstName = this.User.FirstName,
                    LastName = this.User.LastName
                }
            };
        }
    }
}
