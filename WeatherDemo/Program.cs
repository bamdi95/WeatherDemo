using Serilog;

using System.Text.Json.Serialization;

using WeatherDemo.Auth;
using WeatherDemo.Exceptions;
using WeatherDemo.Interfaces;
using WeatherDemo.Options;
using WeatherDemo.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddSerilog((services, lc) => lc
       .ReadFrom.Configuration(builder.Configuration)
       .ReadFrom.Services(services));

    builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

    builder.Services
        .AddOptions<AppSettings>()
        .Bind(builder.Configuration.GetSection(nameof(AppSettings)))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    builder.Services
        .AddOptions<ApiKeySettings>()
        .Bind(builder.Configuration.GetSection(nameof(ApiKeySettings)))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    var appSettings = builder.Configuration
        .GetSection(nameof(AppSettings))
        .Get<AppSettings>() ?? new AppSettings();

    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddHybridCache(options =>
    {
        options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
        {
            LocalCacheExpiration = TimeSpan.FromMinutes(appSettings.CacheExpiryInMinutes),
            Expiration = TimeSpan.FromMinutes(appSettings.CacheExpiryInMinutes),
        };
        options.MaximumPayloadBytes = 1024 * 1024;
    });

    builder.Services.AddHttpClient<IWeatherProvider, OpenMeteoWeatherProvider>(client => { client.BaseAddress = new Uri("https://api.open-meteo.com/"); }).AddStandardResilienceHandler();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services
    .AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
    .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationOptions.DefaultScheme, _ => { });
    builder.Services.AddAuthorization();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();
    app.UseExceptionHandler();
    app.UseStatusCodePages();
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly.");
}
finally
{
    Log.CloseAndFlush();
}