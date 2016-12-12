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
    [Table("pub_ProgramResourceThumb")]
    public class ProgramResourceThumb : BaseEntity<int>
    {
        [Required]
        public byte[] Thumb { get; set; }

        public virtual ProgramResource Resource { get; set; }
    }
}
