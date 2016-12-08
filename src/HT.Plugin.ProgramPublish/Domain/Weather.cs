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
    [Table("pub_Weather")]
    public class Weather : BaseEntity<int>
    {
        /// <summary>
        /// 天气
        /// </summary>
        [MaxLength(20)]
        public string NowWeather { get; set; }

        /// <summary>
        /// 天气编码
        /// </summary>
        [MaxLength(20)]
        public string NowWeatherCode { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        [MaxLength(20)]
        public string Temperature { get; set; }

        /// <summary>
        /// 风力
        /// </summary>
        [MaxLength(20)]
        public string Wind { get; set; }

        /// <summary>
        /// 风力编码
        /// </summary>
        [MaxLength(20)]
        public string WindCode { get; set; }

        /// <summary>
        /// 风向
        /// </summary>
        [MaxLength(20)]
        public string Direction { get; set; }

        /// <summary>
        /// 风向编码
        /// </summary>
        [MaxLength(20)]
        public string DirectionCode { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        [MaxLength(20)]
        public string Humidity { get; set; }

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
    //    "code": 0,
    //    "msg": "Sucess",
    //    "counts": 2362,  //访问的剩余次数。
    //    "data": {
    //        "cityId": "CH010100",  //城市id
    //        "cityName": "北京",  //城市名称
    //        "lastUpdate": "2016-03-09 17:10:00",  //实况更新时间
    //        "tq": "多云",  //天气现象
    //        "numtq": "01",  //天气现象编码
    //        "qw": "5.0",  //当前气温
    //        "fl": "微风",  //当前风力
    //        "numfl": "0",  //当前风力编码
    //        "fx": "无持续风向",  //当前风向
    //        "numfx": "0",  //当前风向编码
    //        "sd": "10.0"  //相对湿度，直接在此数值后添加%即可
    //    }
    //}

}
