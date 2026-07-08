using WeatherDemo.Interfaces;

namespace WeatherDemo.Models
{
    public record WeatherResponse : IWeatherResponse
    {
        public DateTime? DateOfMeasurementUTC { get; set; }

        public double TemperatureCelsius { get; set; }

        public WeatherTpye Weather { get; set; }

        public bool IsDay { get; set; }

        public WeatherResponse(DateTime? dateOfMeasurementUTC, double temperatureC, WeatherTpye weather, bool isDay)
        {
            DateOfMeasurementUTC = dateOfMeasurementUTC;
            TemperatureCelsius = temperatureC;
            Weather = weather;
            IsDay = isDay;
        }

        public WeatherResponse()
        {
            
        }
    }
}
