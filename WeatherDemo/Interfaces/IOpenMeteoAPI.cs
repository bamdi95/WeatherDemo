namespace WeatherDemo.Interfaces
{
    public interface IOpenMeteoAPI
    {
        public Task<IWeatherData> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token);
    }
}
