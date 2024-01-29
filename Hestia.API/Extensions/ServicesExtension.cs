using Hestia.Application.Services;

namespace Hestia.API.Extensions;

public static class ServicesExtension
{
    public static void AddHestiaServices(this IServiceCollection services)
    {
        services.AddScoped<ProductService>();
        services.AddScoped<UserService>();
    }
    
}