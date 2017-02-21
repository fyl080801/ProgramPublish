using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CACSLibrary.Profile;

namespace HT.Plugin.ProgramPublish.Profiles
{
    public class ResourceProfile : BaseProfile
    {
        public string Path { get; set; }

        public string UploadTemp { get; set; }

        public string TerminalFlag { get; set; }

        public string NotifyFlag { get; set; }

        public string WeatherFlag { get; set; }

        public override ProfileObject GetDefault()
        {
            return new ResourceProfile()
            {
                Path = "/Resources",
                UploadTemp = "/UploadTemp",
                TerminalFlag = "/TerminalFlag",
                NotifyFlag = "/NotifyFlag",
                WeatherFlag = "/WeatherFlag"
            };
        }
    }
}
