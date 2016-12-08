using CACSLibrary.Profile;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CACSLibrary.Infrastructure;
using CACSLibrary.Plugin;

namespace HT.Plugin.ProgramPublish.Profiles
{
    public class BaseProfile : ProfileObject
    {
        public BaseProfile()
           : base(new XmlProfileProvider(() =>
           {
               var finder = EngineContext.Current.Resolve<IPluginFinder>();
               var plugin = finder.GetPluginDescriptorById(Plugin.SYSTEM_ID, true);
               return plugin == null ? "" : plugin.PluginFile.DirectoryName;
           }))
        {

        }

        public override ProfileObject GetDefault()
        {
            return this;
        }
    }
}
