namespace WeatherDemo.Interfaces
{
    public interface IWeatherResponse
    {
        DateTime? DateOfMeasurementUTC { get; }
        double TemperatureCelsius { get; }
        string? Weather { get; }
        bool IsDay { get; }
    }
}