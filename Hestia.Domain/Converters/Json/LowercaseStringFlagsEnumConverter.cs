using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hestia.Domain.Converters.Json;

public class LowercaseStringFlagsEnumConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? enumString = reader.GetString();
        if (enumString is null) return default!;

        IEnumerable<T> enumValues = enumString
            .Split(',')
            .Select(s => Enum.Parse(typeToConvert, s.Trim(), true))
            .Cast<T>();

        int combinedValue = enumValues.Cast<int>().Aggregate(0, (acc, val) => acc | val);
        return (T)Enum.ToObject(typeToConvert, combinedValue);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        Type enumType = typeof(T);
        string[] flagValues = Enum.GetValues(enumType)
            .Cast<T>()
            .Where(flag => value.HasFlag(flag) && Convert.ToInt32(flag) != 0) // Exclude 0 value
            .Select(flag => flag.ToString().ToLower())
            .ToArray();

        writer.WriteStartArray();
        foreach (string flag in flagValues)
        {
            writer.WriteStringValue(flag);
        }
        writer.WriteEndArray();
    }
}
