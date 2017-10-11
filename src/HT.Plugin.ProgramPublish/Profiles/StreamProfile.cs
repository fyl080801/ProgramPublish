using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CACSLibrary.Profile;

namespace HT.Plugin.ProgramPublish.Profiles
{
    public class StreamProfile : BaseProfile
    {
        public List<StreamItem> Items { get; set; } = new List<StreamItem>();

        public override ProfileObject GetDefault()
        {
            return new StreamProfile();
        }
    }

    public class StreamItem
    {
        public string Name { get; set; }

        public string Address { get; set; }
    }
}