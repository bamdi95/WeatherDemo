using Microsoft.AspNetCore.Mvc;
using WeatherDemo.Interfaces;

namespace WeatherDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherDemoController : ControllerBase
    {
        private readonly IOpenMeteoAPI _openMeteoAPI;

        public WeatherDemoController(IOpenMeteoAPI openMeteoAPI)
        {
            _openMeteoAPI = openMeteoAPI;
        }

        [HttpGet(Name = "GetCurrentWeather")]
        public async Task<IWeatherData> Get([FromQuery] double latitude, [FromQuery] double longitude, CancellationToken token)
        {
            var result = await _openMeteoAPI.GetWeatherDataAsync(latitude, longitude, token);
            return result;
        }
    }
}
