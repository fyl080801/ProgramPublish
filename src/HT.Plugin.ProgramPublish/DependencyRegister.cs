using CACSLibrary.Infrastructure;
using HT.Plugin.ProgramPublish.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish
{
    public class DependencyRegister : IDependencyRegister
    {
        public EngineLevels Level
        {
            get { return EngineLevels.Normal; }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterComponentInstance<IThumbService>(new ThumbService(), typeof(ThumbService).FullName);
        }
    }
}
