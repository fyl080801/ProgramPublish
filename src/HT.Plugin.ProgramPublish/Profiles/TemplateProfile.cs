using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CACSLibrary.Profile;

namespace HT.Plugin.ProgramPublish.Profiles
{
    public class TemplateProfile : BaseProfile
    {
        public List<Template> Templates { get; set; } = new List<Template>();

        public override ProfileObject GetDefault()
        {
            return new TemplateProfile() { };
        }
    }
}
