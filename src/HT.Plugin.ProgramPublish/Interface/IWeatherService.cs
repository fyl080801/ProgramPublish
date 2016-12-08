using HT.Plugin.ProgramPublish.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Interface
{
    [ServiceContract]
    public interface IWeatherService
    {
        [OperationContract]
        void SaveWeather(WeatherData[] data);

        [OperationContract]
        void SaveForecast(WeatherData[][] data);
    }
}
