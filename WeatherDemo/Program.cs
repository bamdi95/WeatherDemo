using StackExchange.Redis;
using System.Text.Json.Serialization;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;
using WeatherDemo.Models.OpenMeteo;
using WeatherDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonNumberEnumConverter<OpenMeteoWeather>()));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));

builder.Services.AddHttpClient<IWeatherProvider, OpenMeteoAPI>(client => { client.BaseAddress = new Uri("https://api.open-meteo.com/"); });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
