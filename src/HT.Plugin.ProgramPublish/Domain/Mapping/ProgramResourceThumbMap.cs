using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain.Mapping
{
    public class ProgramResourceThumbMap : EntityTypeConfiguration<ProgramResourceThumb>
    {
        public ProgramResourceThumbMap()
        {
            HasKey(e => e.Id);
            HasRequired(e => e.Resource).WithRequiredDependent().WillCascadeOnDelete(true);
        }
    }
}
