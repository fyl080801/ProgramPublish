using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain.Mapping
{
    public class TerminalMap : EntityTypeConfiguration<Terminal>
    {
        public TerminalMap()
        {
            this.HasKey(c => c.Id);
            this.HasMany(e => e.Programs)
                .WithMany(e => e.Terminals)
                .Map(m => m.ToTable("pub_Terminal_Program").MapLeftKey("TerminalId").MapRightKey("ProgramId"));
        }
    }
}
