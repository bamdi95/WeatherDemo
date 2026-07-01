using System.Text.Json;
using System.Text.Json.Serialization;

namespace WeatherDemo.JsonConverters
{
    public class BooleanConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.True:
                    return true;
                case JsonTokenType.False:
                    return false;
                case JsonTokenType.String:
                    return reader.GetString() switch
                    {
                        var value when string.Equals(value, bool.TrueString, StringComparison.OrdinalIgnoreCase) => true,
                        var value when string.Equals(value, bool.FalseString, StringComparison.OrdinalIgnoreCase) => false,
                        _ => throw new JsonException("Could not parse boolean.")
                    };
                case JsonTokenType.Number:
                    return reader.GetInt16() switch
                    {
                        1 => true,
                        0 => false,
                        _ => throw new JsonException("Could not parse boolean.")
                    };
                default:
                    throw new JsonException("Could not parse boolean.");
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
