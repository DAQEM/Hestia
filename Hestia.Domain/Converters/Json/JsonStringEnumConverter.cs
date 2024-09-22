using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hestia.Domain.Converters.Json;

public class JsonStringEnumConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }
    
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return (JsonConverter?) Activator.CreateInstance(
            typeof(JsonStringEnumConverter<>).MakeGenericType(typeToConvert));
    }
}