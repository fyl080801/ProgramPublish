using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain.Mapping
{
    public class GroupUserMap : EntityTypeConfiguration<GroupUser>
    {
        public GroupUserMap()
        {
            this.HasKey(e => e.Id);
            this.HasRequired(e => e.User)
                .WithRequiredDependent()
                .WillCascadeOnDelete(true);
        }
    }
}
