using Hestia.Application.Services.Auth;
using Hestia.Application.Services.Blogs;
using Hestia.Application.Services.Projects;
using Hestia.Application.Services.Users;

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
        services.AddScoped<BlogService>();
        services.AddScoped<BlogCategoryService>();
    }
}