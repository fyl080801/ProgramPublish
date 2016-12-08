using CACSLibrary.Infrastructure;
using HT.Plugin.ProgramPublish.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Services
{
    public class DependencyRegister : IDependencyRegister
    {
        public EngineLevels Level
        {
            get { return EngineLevels.Normal; }
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.RegisterComponent<ITerminalService, TerminalService>(typeof(TerminalService).FullName, ComponentLifeStyle.LifetimeScope);
            containerManager.RegisterComponent<IWeatherService, WeatherService>(typeof(WeatherService).FullName, ComponentLifeStyle.LifetimeScope);
        }
    }
}
