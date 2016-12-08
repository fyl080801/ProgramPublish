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
    [Table("pub_Resource")]
    public class Resource : BaseEntity<int>, ICloneable
    {
        [MaxLength(50), Required]
        public virtual string Name { get; set; }

        public virtual ResourceCategories CategoryId { get; set; }

        public virtual string Content { get; set; }

        [MaxLength(20)]
        public virtual string Mime { get; set; }

        public virtual int UserId { get; set; }

        public virtual DateTime UploadTime { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public object Clone()
        {
            return new Resource()
            {
                CategoryId = this.CategoryId,
                Content = this.Content,
                Id = this.Id,
                Name = this.Name,
                UploadTime = this.UploadTime,
                UserId = this.UserId,
                User = new User()
                {
                    FirstName = this.User.FirstName,
                    LastName = this.User.LastName,
                    Id = this.Id
                }
            };
        }
    }
}
