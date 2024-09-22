using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hestia.Domain.Converters.Json;

public class LowercaseStringEnumConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? enumString = reader.GetString();
        if (enumString is null) return default!;
        return (T)Enum.Parse(typeToConvert, enumString, true);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString().ToLower());
    }
}