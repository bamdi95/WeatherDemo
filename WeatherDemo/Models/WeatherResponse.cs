using WeatherDemo.Interfaces;

namespace WeatherDemo.Models
{
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

        public WeatherResponse()
        {
            
        }
    }
}
