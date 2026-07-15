using System.ComponentModel.DataAnnotations;

namespace WeatherDemo.Options
{
    public sealed class ApiKeySettings
    {
        [Required(AllowEmptyStrings = false)]
        public string ApiKey { get; set; } = string.Empty;
    }
}
