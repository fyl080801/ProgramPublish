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
    [Table("pub_Terminal")]
    public class Terminal : BaseEntity<int>, ICloneable
    {
        [MaxLength(50), Required]
        public string Name { get; set; }

        [MaxLength(255), Required]
        public string TerminalCode { get; set; }

        public int GroupId { get; set; }

        public TerminalTypes TerminalType { get; set; }

        [MaxLength(20)]
        public string IpAddress { get; set; }

        [MaxLength(20)]
        public string MacAddress { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        [MaxLength(255)]
        public string Password { get; set; }

        public int? NotificationId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }

        [ForeignKey("NotificationId")]
        public virtual Notification Notification { get; set; }

        public virtual ICollection<Program> Programs { get; set; } = new HashSet<Program>();

        public object Clone()
        {
            return new Terminal()
            {
                GroupId = this.GroupId,
                Id = this.Id,
                IpAddress = this.IpAddress,
                MacAddress = this.MacAddress,
                Name = this.Name,
                TerminalCode = this.TerminalCode,
                TerminalType = this.TerminalType,
                UserName = this.UserName,
                Password = this.Password,
                NotificationId = this.NotificationId,
                Group = (Group)this.Group.Clone(),
                Notification = this.Notification != null ? (Notification)this.Notification.Clone() : null
            };
        }
    }
}
