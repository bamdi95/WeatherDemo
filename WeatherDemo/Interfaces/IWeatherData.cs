using WeatherDemo.Models.OpenMeteo;

namespace WeatherDemo.Interfaces
{
    public interface IWeatherData
    {
        public DateTime? TimeUTC { get; set; }
        public double TemperatureCelsius {  get; set; }
        public bool IsDay {  get; set; }
        public OpenMeteoWeather Weather { get; set; }
    }
}