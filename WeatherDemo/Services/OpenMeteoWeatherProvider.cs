using System.Globalization;
using System.Text.Json;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;
using WeatherDemo.Models.OpenMeteo;

namespace WeatherDemo.Services
{
    public class OpenMeteoWeatherProvider : IWeatherProvider
    {
        private readonly HttpClient _client;
        private readonly ILogger<OpenMeteoWeatherProvider> _logger;
        private const string RelativePath = "v1/forecast?latitude={0}&longitude={1}&current=temperature_2m,weather_code,is_day&forecast_days=1";

        public OpenMeteoWeatherProvider(HttpClient client, ILogger<OpenMeteoWeatherProvider> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<WeatherResponse> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token)
        {
            _logger.LogInformation("Getting weather data from open-meteo api for {Latitude}, {Longitude}", latitude, longitude);

            var url = new Uri(_client.BaseAddress ?? throw new InvalidOperationException("No base url provided for open meteo call."), string.Format(CultureInfo.InvariantCulture, RelativePath, latitude, longitude));

            using var responseStream = await _client.GetStreamAsync(url, token);

            using var jsonDoc = await JsonDocument.ParseAsync(responseStream, cancellationToken: token);

            if (!jsonDoc.RootElement.TryGetProperty("current", out var currentElement))
            {
                throw new InvalidDataException("OpenMeteoWeatherProvider did not return with expected data.");
            }

            var weatherData = currentElement.Deserialize<OpenMeteoWeatherData>();

            if (weatherData == null)
            {
                throw new InvalidDataException("Failed to deserialize OpenMeteoWeatherProvider weather data.");
            }

            _logger.LogDebug("Got weather data: {@weatherData}", weatherData);

            if (weatherData.TimeUTC.HasValue)
            {
                weatherData.TimeUTC = DateTime.SpecifyKind(weatherData.TimeUTC.Value, DateTimeKind.Utc);
            }

            return new WeatherResponse(weatherData.TimeUTC, weatherData.TemperatureCelsius, MapOpenMeteoWeather(weatherData.Weather), weatherData.IsDay);
        }

        private static WeatherType MapOpenMeteoWeather(OpenMeteoWeather weather) => weather switch
        {
            OpenMeteoWeather.ClearSky or OpenMeteoWeather.MainlyClear => WeatherType.Clear,
            OpenMeteoWeather.PartlyCloudy or OpenMeteoWeather.Overcast => WeatherType.Cloudy,
            OpenMeteoWeather.Fog or OpenMeteoWeather.RimeFog => WeatherType.Fog,
            OpenMeteoWeather.LightDrizzle or OpenMeteoWeather.ModerateDrizzle or OpenMeteoWeather.DenseDrizzle
                or OpenMeteoWeather.LightFreezingDrizzle or OpenMeteoWeather.DenseFreezingDrizzle
                or OpenMeteoWeather.SlightRain or OpenMeteoWeather.ModerateRain or OpenMeteoWeather.HeavyRain
                or OpenMeteoWeather.LightFreezingRain or OpenMeteoWeather.HeavyFreezingRain
                or OpenMeteoWeather.SlightRainShower or OpenMeteoWeather.ModerateRainShower
                or OpenMeteoWeather.ViolentRainShower => WeatherType.Rain,
            OpenMeteoWeather.SlightSnowfall or OpenMeteoWeather.ModerateSnowfall or OpenMeteoWeather.HeavySnowfall
                or OpenMeteoWeather.SnowGrains or OpenMeteoWeather.SlightSnowShower
                or OpenMeteoWeather.HeavySnowShower => WeatherType.Snow,
            OpenMeteoWeather.Thunderstorm => WeatherType.Thunderstorm,
            _ => WeatherType.Unknown
        };
    }
}
