using Hestia.Infrastructure.Application;

namespace Hestia.API.Extensions;

public static class OptionsExtension
{
    public static void AddHestiaOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationSettings>(configuration.GetSection("Application"));
        services.Configure<ApiKeySettings>(configuration.GetSection("ApiKeys"));
    }
}