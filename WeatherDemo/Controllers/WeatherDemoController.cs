using Microsoft.AspNetCore.Mvc;
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

        public WeatherDemoController(IWeatherProvider openMeteoAPI, IOptions<AppSettings> options)
        {
            _openMeteoAPI = openMeteoAPI;
            _roundingDigitPrecision = options.Value.PositionRoundDigitPrecision;
        }

        [HttpGet(Name = "GetCurrentWeather")]
        public async Task<IWeatherResponse> Get([FromQuery] double latitude, [FromQuery] double longitude, CancellationToken token)
        {
            latitude = Math.Round(latitude, _roundingDigitPrecision);
            longitude = Math.Round(longitude, _roundingDigitPrecision);
            var result = await _openMeteoAPI.GetWeatherDataAsync(latitude, longitude, token);
            return new WeatherResponse(result.TimeUTC, result.TemperatureCelsius, result.Weather.ToString(), result.IsDay);
        }
    }
}
