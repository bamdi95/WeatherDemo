using Microsoft.AspNetCore.Authentication;

namespace WeatherDemo.Auth
{
    public sealed class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "ApiKey";
        public const string HeaderName = "X-API-Key";
    }
}
