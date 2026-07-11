using WeatherDemo.Models;

namespace WeatherDemo.Interfaces
{
    public interface IWeatherProvider
    {
        public Task<WeatherResponse> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token);
    }
}
