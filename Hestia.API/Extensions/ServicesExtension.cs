using Hestia.Application.Services;
using Hestia.Application.Services.Auth;

namespace Hestia.API.Extensions;

public static class ServicesExtension
{
    public static void AddHestiaServices(this IServiceCollection services)
    {
        services.AddScoped<ProjectService>();
        services.AddScoped<UserService>();
        services.AddScoped<ProjectCategoryService>();
        services.AddScoped<ProjectSyncService>();
        services.AddScoped<OAuthStateService>();
        services.AddScoped<OAuthProviderService>();
        services.AddScoped<SessionService>();
    }
}