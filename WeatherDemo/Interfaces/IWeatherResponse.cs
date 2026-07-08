using WeatherDemo.Models;

namespace WeatherDemo.Interfaces
{
    public interface IWeatherResponse
    {
        DateTime? DateOfMeasurementUTC { get; }
        double TemperatureCelsius { get; }
        WeatherTpye Weather { get; }
        bool IsDay { get; }
    }
}