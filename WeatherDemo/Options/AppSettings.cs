using System.ComponentModel.DataAnnotations;

namespace WeatherDemo.Options
{
    public class AppSettings
    {
        [Range(0, 15)]
        public int PositionRoundDigitPrecision { get; set; }
        [Range(1, 1440)]
        public int CacheExpiryInMinutes { get; set; }
    }
}
