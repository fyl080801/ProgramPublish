using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain.Mapping
{
    public class ProgramExamineMap : EntityTypeConfiguration<ProgramExamine>
    {
        public ProgramExamineMap()
        {
            this.HasKey(c => c.Id);
        }
    }
}
