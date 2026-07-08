using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
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
        [HttpGet("{latitude}/{longitude}")]
        public async Task<IWeatherResponse> Get([FromRoute][Range(-90, 90)] double latitude, [FromRoute][Range(-180, 180)] double longitude, CancellationToken token)
        {
            latitude = Math.Round(latitude, _roundingDigitPrecision);
            longitude = Math.Round(longitude, _roundingDigitPrecision);

            var key = FormattableString.Invariant($"{latitude}:{longitude}");

            return await _cache.GetOrCreateAsync(key,
                async cancel => await _openMeteoAPI.GetWeatherDataAsync(latitude, longitude, cancel), cancellationToken: token);
        }
    }
}
