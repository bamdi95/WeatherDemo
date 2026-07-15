using System.Text.Json.Serialization;
using WeatherDemo.Interfaces;

namespace WeatherDemo.Models.OpenMeteo
{
    public class OpenMeteoWeatherData : IWeatherData
    {
        [JsonPropertyName("time")]
        public DateTime? TimeUTC { get; set; }

        [JsonPropertyName("temperature_2m")]
        [JsonRequired]
        public double TemperatureCelsius { get; set; }

        [JsonPropertyName("is_day")]
        [JsonRequired]
        [JsonConverter(typeof(JsonConverters.BooleanConverter))]
        public bool IsDay { get; set; }

        [JsonPropertyName("weather_code")]
        [JsonRequired]
        [JsonConverter(typeof(JsonNumberEnumConverter<OpenMeteoWeather>))]
        public OpenMeteoWeather Weather { get; set; }
    }
}
