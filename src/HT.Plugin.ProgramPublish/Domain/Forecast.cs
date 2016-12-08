using CACSLibrary.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HT.Plugin.ProgramPublish.Domain
{
    [Table("pub_Forecast")]
    public class Forecast : BaseEntity<int>
    {
        /// <summary>
        /// 白天天气
        /// </summary>
        [MaxLength(20)]
        public string DayWeather { get; set; }

        /// <summary>
        /// 白天天气编码
        /// </summary>
        [MaxLength(20)]
        public string DayWeatherCode { get; set; }

        /// <summary>
        /// 白天气温
        /// </summary>
        [MaxLength(20)]
        public string DayTemperature { get; set; }

        /// <summary>
        /// 白天风力
        /// </summary>
        [MaxLength(20)]
        public string DayWind { get; set; }

        /// <summary>
        /// 白天风力编码
        /// </summary>
        [MaxLength(20)]
        public string DayWindCode { get; set; }

        /// <summary>
        /// 白天风向
        /// </summary>
        [MaxLength(20)]
        public string DayDirection { get; set; }

        /// <summary>
        /// 白天风向编码
        /// </summary>
        [MaxLength(20)]
        public string DayDirectionCode { get; set; }

        /// <summary>
        /// 夜间天气
        /// </summary>
        [MaxLength(20)]
        public string NightWeather { get; set; }

        /// <summary>
        /// 夜间天气编码
        /// </summary>
        [MaxLength(20)]
        public string NightWeatherCode { get; set; }

        /// <summary>
        /// 夜间气温
        /// </summary>
        [MaxLength(20)]
        public string NightTemperature { get; set; }

        /// <summary>
        /// 夜间风力
        /// </summary>
        [MaxLength(20)]
        public string NightWind { get; set; }

        /// <summary>
        /// 夜间风力编码
        /// </summary>
        [MaxLength(20)]
        public string NightWindCode { get; set; }

        /// <summary>
        /// 夜间风向
        /// </summary>
        [MaxLength(20)]
        public string NightDirection { get; set; }

        /// <summary>
        /// 夜间风向编码
        /// </summary>
        [MaxLength(20)]
        public string NightDirectionCode { get; set; }

        /// <summary>
        /// 预报日期
        /// </summary>
        public DateTime ForecastDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [MaxLength(20)]
        public string CityName { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        [MaxLength(20)]
        public string CityId { get; set; }
    }

    //{
    //    "code": 1,
    //    "msg": "Sucess",
    //    "counts": 2362,  //访问的剩余次数。
    //    "data": {
    //        "cityId": "CH010100",  //城市id
    //        "cityName": "北京",  //城市名称
    //        "sj": "2016-03-09 17:00:00",  //数据更新时间
    //        "list": [
    //            {
    //                "tq1": "多云",  //白天天气
    //                "tq2": "晴",  //夜间天气，当与白天天气相同时，两者可合并为一个
    //                "numtq1": "01",  //白天天气编码
    //                "numtq2": "00",  //夜间天气编码
    //                "qw1": "6",  //白天气温
    //                "qw2": "-5",  //夜间气温
    //                "fl1": "3-4级",  //白天风力
    //                "numfl1": "1",  //白天风力编码
    //                "fl2": "微风",  //夜间风力
    //                "numfl2": "0",  //夜间风力编码
    //                "fx1": "北风",  //白天风向
    //                "numfx1": "8",  //白天风向编码
    //                "fx2": "无持续风向",  //夜间风向，如白天风力风向与夜间风力风向一致，可合并为一行
    //                "numfx2": "0",  //夜间风向编码
    //                "date": "2016-03-09"  //预报日期
    //            }
    //        ]
    //    }
    //}
}
