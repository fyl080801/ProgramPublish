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
    [Table("pub_Group")]
    public class Group : BaseEntity<int>, ICloneable
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int? ParentId { get; set; }

        [MaxLength(255)]
        public string RelationPath { get; set; }

        [ForeignKey("ParentId")]
        public virtual Group Parent { get; set; }

        public virtual ICollection<Terminal> Terminals { get; set; } = new HashSet<Terminal>();

        public object Clone()
        {
            return new Group()
            {
                Id = this.Id,
                Name = this.Name,
                ParentId = this.ParentId,
                RelationPath = this.RelationPath
            };
        }
    }
}
