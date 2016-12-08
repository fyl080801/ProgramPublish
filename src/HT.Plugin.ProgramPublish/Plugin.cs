using CACS.Framework.Data;
using CACS.Framework.Plugin;
using CACSLibrary.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish
{
    public class Plugin : CACSWebPlugin
    {
        public const string SYSTEM_ID = "HT.Plugin.ProgramPublish";

        IDataSettingsManager _dbSettingManager;

        public Plugin(
            IDataSettingsManager dbSettingManager)
        {
            _dbSettingManager = dbSettingManager;
        }

        public override void Install()
        {
            base.Install();

            var setting = _dbSettingManager.LoadSettings();
            if (!setting.EntityMapAssmbly.Contains("HT.Plugin.ProgramPublish"))
                setting.EntityMapAssmbly.Add("HT.Plugin.ProgramPublish");
            _dbSettingManager.SaveSettings(setting);

            base.AddWcfService(
                "HT.Plugin.ProgramPublish.Interface.IWeatherService",
                "HT.Plugin.ProgramPublish.Services.WeatherService",
                "ProgramPublish/WeatherService.svc");
        }

        public override void Uninstall()
        {
            base.Uninstall();

            var setting = _dbSettingManager.LoadSettings();
            setting.EntityMapAssmbly.Remove("HT.Plugin.ProgramPublish");
            _dbSettingManager.SaveSettings(setting);

            base.RemoveWcfService("HT.Plugin.ProgramPublish.Services.WeatherService");
        }
    }
}
