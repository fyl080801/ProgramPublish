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
    [Table("pub_Program")]
    public class Program : BaseEntity<int>, ICloneable
    {
        [MaxLength(50), Required]
        public string Name { get; set; }//nvarchar	50			FALSE 节目名称

        public string Remark { get; set; }// nvarchar    max TRUE    描述

        public DateTime StartTime { get; set; }      // nvarchar FALSE   开始时间 如果一个终端分时段有多套节目，以开始时间为准播出对应节目

        public DateTime? EndTime { get; set; }//     nvarchar TRUE    结束时间 如果存在结束时间则在到时间后结束节目播放

        public DateTime UpdateTime { get; set; }// nvarchar                FALSE 修改时间    节目最近修改时间

        public bool IsUpdated { get; set; }//       nvarchar		0		FALSE 是否已更新	0-未更新，1-已更新

        public ProgramStates State { get; set; }

        [MaxLength(50)]
        public string Template { get; set; }

        public bool Enabled { get; set; }//     nvarchar		1		FALSE 是否启用	0-禁用,1-启用(启用的节目才可播放)

        public virtual ICollection<ProgramResource> Resources { get; set; } = new HashSet<ProgramResource>();

        public virtual ICollection<Terminal> Terminals { get; set; } = new HashSet<Terminal>();

        public object Clone()
        {
            return new Program()
            {
                State = this.State,
                Enabled = this.Enabled,
                EndTime = this.EndTime,
                Id = this.Id,
                IsUpdated = this.IsUpdated,
                Name = this.Name,
                Remark = this.Remark,
                StartTime = this.StartTime,
                UpdateTime = this.UpdateTime,
                Template = this.Template
            };
        }
    }
}
