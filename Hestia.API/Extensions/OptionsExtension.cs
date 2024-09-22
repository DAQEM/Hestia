using Hestia.Application.Options;
using Hestia.Infrastructure.Options;

namespace Hestia.API.Extensions;

public static class OptionsExtension
{
    public static void AddHestiaOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationOptions>(configuration.GetSection(ApplicationOptions.Section));
        services.Configure<ApiKeyOptions>(configuration.GetSection(ApiKeyOptions.Section));
        services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.Section));
    }
}