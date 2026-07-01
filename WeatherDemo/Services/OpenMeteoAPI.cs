using System.Globalization;
using System.Text.Json;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;

namespace WeatherDemo.Services
{
    public class OpenMeteoAPI : IOpenMeteoAPI
    {
        private readonly HttpClient _client;
        private readonly ILogger<OpenMeteoAPI> _logger;
        private readonly string _relativePath = "v1/forecast?latitude={0}&longitude={1}&current=temperature_2m,weather_code,is_day&forecast_days=1";

        public OpenMeteoAPI(HttpClient client, ILogger<OpenMeteoAPI> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IWeatherData> GetWeatherDataAsync(double latitude, double longitude, CancellationToken token)
        {
            _logger.LogInformation("Getting weather data from open-meteo api for {Latitude}, {Longitude}", latitude, longitude);

            var url = new Uri(_client.BaseAddress ?? throw new BadHttpRequestException("No base url provided for open meteo call."), string.Format(CultureInfo.InvariantCulture, _relativePath, latitude, longitude));

            using var responseStream = await _client.GetStreamAsync(url, token);

            using var jsonDoc = await JsonDocument.ParseAsync(responseStream, cancellationToken: token);

            if (!jsonDoc.RootElement.TryGetProperty("current", out var currentElement))
            {
                throw new InvalidDataException("OpenMeteoAPI did not return with expected data.");
            }

            var weatherData = currentElement.Deserialize<WeatherData>();

            if (weatherData == null)
            {
                throw new InvalidDataException("Failed to deserialize weather data.");
            }

            if (weatherData.TimeUTC.HasValue)
            {
                weatherData.TimeUTC = DateTime.SpecifyKind(weatherData.TimeUTC.Value, DateTimeKind.Utc);
            }

            return weatherData;
        }
    }
}
