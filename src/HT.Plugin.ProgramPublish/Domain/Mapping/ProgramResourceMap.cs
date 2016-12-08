using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain.Mapping
{
    public class ProgramResourceMap : EntityTypeConfiguration<ProgramResource>
    {
        public ProgramResourceMap()
        {
            this.HasKey(c => c.Id);
        }
    }
}
