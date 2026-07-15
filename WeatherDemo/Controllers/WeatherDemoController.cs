using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;
using WeatherDemo.Options;

namespace WeatherDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherDemoController : ControllerBase
    {
        private readonly IWeatherProvider _weatherProvider;
        private readonly int _roundingDigitPrecision;
        private readonly HybridCache _cache;

        public WeatherDemoController(IWeatherProvider weatherProvider, IOptions<AppSettings> options, HybridCache cache)
        {
            _weatherProvider = weatherProvider;
            _roundingDigitPrecision = options.Value.PositionRoundDigitPrecision;
            _cache = cache;
        }

        [HttpGet("{latitude}/{longitude}", Name = "GetCurrentWeatherForPosition")]
        public async Task<WeatherResponse> Get([FromRoute][Range(-90, 90)] double latitude, [FromRoute][Range(-180, 180)] double longitude, CancellationToken token)
        {
            latitude = Math.Round(latitude, _roundingDigitPrecision);
            longitude = Math.Round(longitude, _roundingDigitPrecision);

            var key = FormattableString.Invariant($"{latitude}:{longitude}");

            return await _cache.GetOrCreateAsync(key,
                async cancel => await _weatherProvider.GetWeatherDataAsync(latitude, longitude, cancel), cancellationToken: token);
        }
    }
}
