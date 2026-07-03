namespace WeatherDemo.Interfaces
{
    public interface IWeatherProvider
    {
        public Task<IOpenMeteoWeatherData> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token);
    }
}
