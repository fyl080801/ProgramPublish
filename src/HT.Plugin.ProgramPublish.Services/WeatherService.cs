using CACSLibrary.Data;
using CACSLibrary.Profile;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.Interface;
using HT.Plugin.ProgramPublish.Profiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Services
{
    public class WeatherService : IWeatherService
    {
        IRepository<Weather> _weatherRepository;
        IRepository<Forecast> _forecastRepository;
        IProfileManager _profileManager;

        public WeatherService(
            IRepository<Weather> weatherRepository,
            IRepository<Forecast> forecastRepository,
            IProfileManager profileManager)
        {
            _weatherRepository = weatherRepository;
            _forecastRepository = forecastRepository;
            _profileManager = profileManager;
        }

        public void SaveForecast(WeatherData[][] data)
        {
            IList<Forecast> casts = new List<Forecast>();
            data.ForEach(cast =>
            {
                IDictionary<string, string> dic = new Dictionary<string, string>();
                cast.ForEach(item =>
                {
                    if (!dic.ContainsKey(item.Key)) dic.Add(item.Key, item.Value);
                });
                casts.Add(new Forecast()
                {
                    CityId = dic["CityId"],
                    CityName = dic["CityName"],
                    DayDirection = dic["DayDirection"],
                    DayDirectionCode = dic["DayDirectionCode"],
                    DayTemperature = dic["DayTemperature"],
                    DayWeather = dic["DayWeather"],
                    DayWeatherCode = dic["DayWeatherCode"],
                    DayWind = dic["DayWind"],
                    DayWindCode = dic["DayWindCode"],
                    ForecastDate = Convert.ToDateTime(dic["ForecastDate"]),
                    NightDirection = dic["NightDirection"],
                    NightDirectionCode = dic["NightDirectionCode"],
                    NightTemperature = dic["NightTemperature"],
                    NightWeather = dic["NightWeather"],
                    NightWeatherCode = dic["NightWeatherCode"],
                    NightWind = dic["NightWind"],
                    NightWindCode = dic["NightWindCode"],
                    UpdateTime = Convert.ToDateTime(dic["UpdateTime"])
                });
            });
            _forecastRepository.Delete(_forecastRepository.Table.ToArray());
            _forecastRepository.Insert(casts.ToArray());

            var profile = _profileManager.Get<ResourceProfile>();
            if (!Directory.Exists(profile.WeatherFlag)) Directory.CreateDirectory(profile.WeatherFlag);
            using (var sw = new StreamWriter(new FileStream(
                    string.Format("{0}/{1}.txt", profile.WeatherFlag, "Forecast"),
                    FileMode.Create, FileAccess.ReadWrite)))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Close();
            }
        }

        public void SaveWeather(WeatherData[] data)
        {
            IDictionary<string, string> dic = new Dictionary<string, string>();
            data.ForEach(item =>
            {
                if (!dic.ContainsKey(item.Key)) dic.Add(item.Key, item.Value);
            });
            var weather = new Weather()
            {
                CityId = dic["CityId"],
                CityName = dic["CityName"],
                Direction = dic["Direction"],
                DirectionCode = dic["DirectionCode"],
                Humidity = dic["Humidity"],
                NowWeather = dic["NowWeather"],
                NowWeatherCode = dic["NowWeatherCode"],
                Temperature = dic["Temperature"],
                UpdateTime = Convert.ToDateTime(dic["UpdateTime"]),
                Wind = dic["Wind"],
                WindCode = dic["WindCode"]
            };
            _weatherRepository.Delete(_weatherRepository.Table.ToArray());
            _weatherRepository.Insert(weather);

            var profile = _profileManager.Get<ResourceProfile>();
            if (!Directory.Exists(profile.WeatherFlag)) Directory.CreateDirectory(profile.WeatherFlag);
            using (var sw = new StreamWriter(new FileStream(
                    string.Format("{0}/{1}.txt", profile.WeatherFlag, "Weather"),
                    FileMode.Create, FileAccess.ReadWrite)))
            {
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Close();
            }
        }
    }
}
