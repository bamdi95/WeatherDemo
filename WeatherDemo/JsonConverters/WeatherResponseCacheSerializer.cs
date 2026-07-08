using Microsoft.Extensions.Caching.Hybrid;
using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;
using WeatherDemo.Interfaces;
using WeatherDemo.Models;

namespace WeatherDemo.JsonConverters
{
    public class WeatherResponseCacheSerializer : IHybridCacheSerializer<IWeatherResponse>
    {
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            Converters = { new JsonStringEnumConverter() }
        };

        public IWeatherResponse Deserialize(ReadOnlySequence<byte> source)
        {
            var reader = new Utf8JsonReader(source);
            return JsonSerializer.Deserialize<WeatherResponse>(ref reader, jsonSerializerOptions) ?? throw new JsonException("Could not deserialize weather response");
        }

        public void Serialize(IWeatherResponse value, IBufferWriter<byte> target)
        {
            if (value is not WeatherResponse)
            {
                throw new JsonException("Could not serialize weather response");
            }
            using var writer = new Utf8JsonWriter(target);
            JsonSerializer.Serialize(writer, (WeatherResponse)value, jsonSerializerOptions);
        }
    }
}
