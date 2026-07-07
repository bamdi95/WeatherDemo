using Microsoft.Extensions.Caching.Hybrid;
using System.Buffers;
using System.Text.Json;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;

namespace WeatherDemo.JsonConverters
{
    public class WeatherResponseCacheSerializer : IHybridCacheSerializer<IWeatherResponse>
    {
        public IWeatherResponse Deserialize(ReadOnlySequence<byte> source)
        {
            var reader = new Utf8JsonReader(source);
            return JsonSerializer.Deserialize<WeatherResponse>(ref reader, new JsonSerializerOptions()) ?? throw new JsonException("Could not deserialize");
        }

        public void Serialize(IWeatherResponse value, IBufferWriter<byte> target)
        {
            using var writer = new Utf8JsonWriter(target);
            JsonSerializer.Serialize(writer, (WeatherResponse)value, new JsonSerializerOptions());
        }
    }
}
