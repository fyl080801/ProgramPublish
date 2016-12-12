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
    [Table("pub_ProgramThumb")]
    public class ProgramThumb : BaseEntity<int>
    {
        [Required]
        public byte[] Thumb { get; set; }

        public virtual Program Program { get; set; }
    }
}
