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
    [Table("pub_ProgramResource")]
    public class ProgramResource : BaseEntity, ICloneable
    {
        [MaxLength(50)]
        public virtual string Name { get; set; }

        [MaxLength(255)]
        public virtual string Mime { get; set; }

        public int ProgramId { get; set; }// int FALSE   节目Id

        public ResourceCategories CategoryId { get; set; }//      nvarchar	2			FALSE 素材类别	1-pptx、2-flash、3-视频、4-图片、5-网址、6-文字

        public int? Duration { get; set; }

        public int OrderIndex { get; set; }

        public string Content { get; set; }//     nvarchar max         TRUE 内容  作为文件类就是路径，作为网址就是链接地址，作为文字就是文本内容

        [MaxLength(50), Required]
        public string DisplayId { get; set; } = "1";//       nvarchar	50			FALSE 显示位置Id

        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        public object Clone()
        {
            return new ProgramResource()
            {
                Name = this.Name,
                Mime = this.Mime,
                CategoryId = this.CategoryId,
                Content = this.Content,
                DisplayId = this.DisplayId,
                Id = this.Id,
                ProgramId = this.ProgramId,
                Duration = this.Duration,
                OrderIndex = this.OrderIndex
            };
        }
    }
}
