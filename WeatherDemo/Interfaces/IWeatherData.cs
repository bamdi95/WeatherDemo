namespace WeatherDemo.Interfaces
{
    public interface IWeatherData
    {
        public DateTime? TimeUTC { get; set; }
        public double TemperatureCelsius {  get; set; }
        public bool IsDay {  get; set; }
        public int WeatherCode { get; set; }
    }
}