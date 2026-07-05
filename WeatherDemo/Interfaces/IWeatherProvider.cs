namespace WeatherDemo.Interfaces
{
    public interface IWeatherProvider
    {
        public Task<IWeatherData> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token);
    }
}
