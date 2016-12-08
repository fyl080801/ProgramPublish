using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Profiles
{
    public class Template
    {
        public string Name { get; set; }

        public List<Area> Areas { get; set; } = new List<Area>();
    }

    public class Area
    {
        public string Name { get; set; }

        public string Code { get; set; }
    }
}
