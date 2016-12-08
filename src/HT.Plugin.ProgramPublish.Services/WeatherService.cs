using CACSLibrary.Data;
using HT.Plugin.ProgramPublish.Domain;
using HT.Plugin.ProgramPublish.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Services
{
    public class WeatherService : IWeatherService
    {
        IRepository<Weather> _weatherRepository;
        IRepository<Forecast> _forecastRepository;

        public WeatherService(
            IRepository<Weather> weatherRepository,
            IRepository<Forecast> forecastRepository)
        {
            _weatherRepository = weatherRepository;
            _forecastRepository = forecastRepository;
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
        }
    }
}
