namespace WeatherDemo.Models
{
    public record WeatherResponse(
        DateTime? DateOfMeasurementUTC,
        double TemperatureCelsius,
        WeatherType Weather,
        bool IsDay);
}
