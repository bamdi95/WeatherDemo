using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;

namespace WeatherDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherDemoController : ControllerBase
    {
        private readonly IWeatherProvider _openMeteoAPI;
        private readonly int _roundingDigitPrecision;
        private readonly IDatabase _redis;

        public WeatherDemoController(IWeatherProvider openMeteoAPI, IOptions<AppSettings> options, IConnectionMultiplexer muxer)
        {
            _openMeteoAPI = openMeteoAPI;
            _roundingDigitPrecision = options.Value.PositionRoundDigitPrecision;
            _redis = muxer.GetDatabase();
        }

        [HttpGet(Name = "GetCurrentWeather")]
        public async Task<IWeatherResponse> Get([FromQuery] double latitude, [FromQuery] double longitude, CancellationToken token)
        {
            latitude = Math.Round(latitude, _roundingDigitPrecision);
            longitude = Math.Round(longitude, _roundingDigitPrecision);

            var key = $"{latitude}:{longitude}";

            var json = await _redis.StringGetAsync(key);
            IWeatherResponse? response;
            if (json.HasValue)
            {
                response = JsonSerializer.Deserialize<WeatherResponse>(json.ToString());
                if (response != null)
                {
                    return response;
                }
            }

            var result = await _openMeteoAPI.GetWeatherDataAsync(latitude, longitude, token);
            response = new WeatherResponse(result.TimeUTC, result.TemperatureCelsius, result.Weather.ToString(), result.IsDay);
            var setTask = _redis.StringSetAsync(key, JsonSerializer.Serialize<IWeatherResponse>(response));
            var expireTask = _redis.KeyExpireAsync(key, TimeSpan.FromSeconds(3600));
            await Task.WhenAll(setTask, expireTask);
            return response;
        }
    }
}
