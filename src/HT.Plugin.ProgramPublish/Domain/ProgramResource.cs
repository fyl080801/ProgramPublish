﻿using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain
{
    [Table("pub_ProgramResource")]
    public class ProgramResource : BaseEntity, ICloneable
    {
        public int ProgramId { get; set; }// int FALSE   节目Id

        public ResourceCategories CategoryId { get; set; }//      nvarchar	2			FALSE 素材类别	1-pptx、2-flash、3-视频、4-图片、5-网址、6-文字

        public string Content { get; set; }//     nvarchar max         TRUE 内容  作为文件类就是路径，作为网址就是链接地址，作为文字就是文本内容

        [MaxLength(50), Required]
        public string DisplayId { get; set; }//       nvarchar	50			FALSE 显示位置Id

        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        public object Clone()
        {
            return new ProgramResource()
            {
                CategoryId = this.CategoryId,
                Content = this.Content,
                DisplayId = this.DisplayId,
                Id = this.Id,
                ProgramId = this.ProgramId
            };
        }
    }
}
