using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
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
        private readonly HybridCache _cache;

        public WeatherDemoController(IWeatherProvider openMeteoAPI, IOptions<AppSettings> options, HybridCache cache)
        {
            _openMeteoAPI = openMeteoAPI;
            _roundingDigitPrecision = options.Value.PositionRoundDigitPrecision;
            _cache = cache;
        }

        [HttpGet(Name = "GetCurrentWeather")]
        public async Task<IWeatherResponse> Get([FromQuery] double latitude, [FromQuery] double longitude, CancellationToken token)
        {
            if (latitude > 90 || latitude < -90 || longitude > 180 || longitude < -180)
            {
                throw new InvalidOperationException("Invalid latitude, longitude values");
            }

            latitude = Math.Round(latitude, _roundingDigitPrecision);
            longitude = Math.Round(longitude, _roundingDigitPrecision);

            var key = FormattableString.Invariant($"{latitude}:{longitude}");

            return await _cache.GetOrCreateAsync(key,
                async cancel => await _openMeteoAPI.GetWeatherDataAsync(latitude, longitude, cancel), cancellationToken: token);
        }
    }
}
