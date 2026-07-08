using System.Globalization;
using System.Text.Json;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;
using WeatherDemo.Models.OpenMeteo;

namespace WeatherDemo.Services
{
    public class OpenMeteoAPI : IWeatherProvider
    {
        private readonly HttpClient _client;
        private readonly ILogger<OpenMeteoAPI> _logger;
        private const string _relativePath = "v1/forecast?latitude={0}&longitude={1}&current=temperature_2m,weather_code,is_day&forecast_days=1";

        public OpenMeteoAPI(HttpClient client, ILogger<OpenMeteoAPI> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IWeatherResponse> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token)
        {
            _logger.LogInformation("Getting weather data from open-meteo api for {Latitude}, {Longitude}", latitude, longitude);

            var url = new Uri(_client.BaseAddress ?? throw new InvalidOperationException("No base url provided for open meteo call."), string.Format(CultureInfo.InvariantCulture, _relativePath, latitude, longitude));

            using var responseStream = await _client.GetStreamAsync(url, token);

            using var jsonDoc = await JsonDocument.ParseAsync(responseStream, cancellationToken: token);

            if (!jsonDoc.RootElement.TryGetProperty("current", out var currentElement))
            {
                throw new InvalidDataException("OpenMeteoAPI did not return with expected data.");
            }

            var weatherData = currentElement.Deserialize<OpenMeteoWeatherData>();

            if (weatherData == null)
            {
                throw new InvalidDataException("Failed to deserialize OpenMeteoAPI weather data.");
            }

            if (weatherData.TimeUTC.HasValue)
            {
                weatherData.TimeUTC = DateTime.SpecifyKind(weatherData.TimeUTC.Value, DateTimeKind.Utc);
            }

            return new WeatherResponse(weatherData.TimeUTC, weatherData.TemperatureCelsius, MapOpenMeteoWeather(weatherData.Weather), weatherData.IsDay);
        }

        private WeatherTpye MapOpenMeteoWeather(OpenMeteoWeather weather)
        {
            switch (weather)
            {
                case OpenMeteoWeather.ClearSky:
                case OpenMeteoWeather.MainlyClear:
                    return WeatherTpye.Clear;
                case OpenMeteoWeather.PartlyCloudy:
                case OpenMeteoWeather.Overcast:
                    return WeatherTpye.Cloudy;
                case OpenMeteoWeather.Fog:
                case OpenMeteoWeather.RimeFog:
                    return WeatherTpye.Fog;
                case OpenMeteoWeather.LightDrizzle:
                case OpenMeteoWeather.ModerateDrizzle:
                case OpenMeteoWeather.DenseDrizzle:
                case OpenMeteoWeather.LightFreezingDrizzle:
                case OpenMeteoWeather.DenseFreezingDrizzle:
                case OpenMeteoWeather.SlightRain:
                case OpenMeteoWeather.ModerateRain:
                case OpenMeteoWeather.HeavyRain:
                case OpenMeteoWeather.LightFreezingRain:
                case OpenMeteoWeather.HeavyFreezingRain:
                case OpenMeteoWeather.SlightRainShower:
                case OpenMeteoWeather.ModerateRainShower:
                case OpenMeteoWeather.ViolentRainShower:
                    return WeatherTpye.Rain;
                case OpenMeteoWeather.SlightSnowfall:
                case OpenMeteoWeather.ModerateSnowfall:
                case OpenMeteoWeather.HeavySnowfall:
                case OpenMeteoWeather.SnowGrains:
                case OpenMeteoWeather.SlightSnowShower:
                case OpenMeteoWeather.HeavySnowShower:
                    return WeatherTpye.Snow;
                case OpenMeteoWeather.Thunderstorm:
                    return WeatherTpye.Thunderstorm;
                default: return WeatherTpye.Unknown;
            }
        }
    }
}
