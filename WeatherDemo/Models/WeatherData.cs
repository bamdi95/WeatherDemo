using System.Text.Json.Serialization;
using WeatherDemo.Interfaces;

namespace WeatherDemo.Models
{
    public class WeatherData : IWeatherData
    {
        [JsonPropertyName("time")]
        public DateTime? TimeUTC { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double TemperatureCelsius { get; set; }

        [JsonPropertyName("is_day")]
        [JsonConverter(typeof(JsonConverters.BooleanConverter))]
        public bool IsDay { get; set; }

        [JsonPropertyName("weather_code")]
        [JsonConverter(typeof(JsonNumberEnumConverter<Weather>))]
        public Weather Weather { get; set; }
    }

    public class WeatherResponse : IWeatherResponse
    {
        public DateTime? DateOfMeasurementUTC { get; set; }

        public double TemperatureCelsius { get; set; }

        public string? Weather { get; set; }

        public bool IsDay { get; set; }

        public WeatherResponse(DateTime? dateOfMeasurementUTC, double temperatureC, string? weather, bool isDay)
        {
            DateOfMeasurementUTC = dateOfMeasurementUTC;
            TemperatureCelsius = temperatureC;
            Weather = weather;
            IsDay = isDay;
        }
    }

    public enum Weather
    {
        ClearSky = 0,
        MainlyClear = 1,
        PartlyCloudy = 2,
        Overcast = 3,
        Fog = 45,
        RimeFog = 48,
        LightDrizzle = 51,
        ModerateDrizzle = 53,
        DenseDrizzle = 55,
        LightFreezingDrizzle = 56,
        DenseFreezingDrizzle = 57,
        SlightRain = 61,
        ModerateRain = 63,
        HeavyRain = 65,
        LightFreezingRain = 66,
        HeavyFreezingRain = 67,
        SlightSnowfall = 71,
        ModerateSnowfall = 73,
        HeavySnowfall = 75,
        SnowGrains = 77,
        SlightRainShower = 80,
        ModerateRainShower = 81,
        ViolentRainShower = 82,
        SlightSnowShower = 85,
        HeavySnowShower = 86,
        Thunderstorm = 95,
    }
}
