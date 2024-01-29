using System.Text.Json;

namespace Hestia.API.Extensions;

public static class JsonExtension
{
    public static void AddHestiaJson(this IServiceCollection services)
    {
        services.ConfigureHttpJsonOptions(options => options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);
    }

    public static void AddHestiaJson(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);
    }
}