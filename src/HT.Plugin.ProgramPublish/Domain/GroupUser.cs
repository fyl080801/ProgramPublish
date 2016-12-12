using CACS.Framework.Domain;
using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain
{
    [Table("pub_GroupUser")]
    public class GroupUser : BaseEntity<int>
    {
        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        public virtual User User { get; set; }
    }
}
