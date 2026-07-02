namespace WeatherDemo.Interfaces
{
    public interface IWeatherProviderAPI
    {
        public Task<IWeatherData> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token);
    }
}
