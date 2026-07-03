using System.Text.Json.Serialization;
using WeatherDemo.Interfaces;

namespace WeatherDemo.Models.OpenMeteo
{
    public class OpenMeteoWeatherData : IOpenMeteoWeatherData
    {
        [JsonPropertyName("time")]
        public DateTime? TimeUTC { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double TemperatureCelsius { get; set; }

        [JsonPropertyName("is_day")]
        [JsonConverter(typeof(JsonConverters.BooleanConverter))]
        public bool IsDay { get; set; }

        [JsonPropertyName("weather_code")]
        [JsonConverter(typeof(JsonNumberEnumConverter<OpenMeteoWeather>))]
        public OpenMeteoWeather Weather { get; set; }
    }
}
