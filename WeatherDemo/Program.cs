using Serilog;
using System.Text.Json.Serialization;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;
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

    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = "localhost:6379"; });
    builder.Services.AddHybridCache(options =>
    {
        options.DefaultEntryOptions = new Microsoft.Extensions.Caching.Hybrid.HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromMinutes(15),
            LocalCacheExpiration = TimeSpan.FromMinutes(15),
        };
    });

    builder.Services.AddHttpClient<IWeatherProvider, OpenMeteoAPI>(client => { client.BaseAddress = new Uri("https://api.open-meteo.com/"); }).AddStandardResilienceHandler();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

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