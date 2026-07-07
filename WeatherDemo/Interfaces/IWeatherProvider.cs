namespace WeatherDemo.Interfaces
{
    public interface IWeatherProvider
    {
        public Task<IWeatherResponse> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token);
    }
}
