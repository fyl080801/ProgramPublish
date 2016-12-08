using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain
{
    [Table("pub_Notification")]
    public class Notification : BaseEntity<int>, ICloneable
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public virtual ICollection<Terminal> Terminals { get; set; } = new HashSet<Terminal>();

        public object Clone()
        {
            return new Notification()
            {
                Content = this.Content,
                EndTime = this.EndTime,
                StartTime = this.StartTime,
                Id = this.Id,
                Title = this.Title
            };
        }
    }
}