using WeatherDemo.Interfaces;

namespace WeatherDemo.Models
{
    public record WeatherResponse
    {
        public DateTime? DateOfMeasurementUTC { get; set; }

        public double TemperatureCelsius { get; set; }

        public WeatherTpye Weather { get; set; }

        public bool IsDay { get; set; }

        public WeatherResponse(DateTime? DateOfMeasurementUTC, double TemperatureCelsius, WeatherTpye Weather, bool IsDay)
        {
            this.DateOfMeasurementUTC = DateOfMeasurementUTC;
            this.TemperatureCelsius = TemperatureCelsius;
            this.Weather = Weather;
            this.IsDay = IsDay;
        }
    }
}
