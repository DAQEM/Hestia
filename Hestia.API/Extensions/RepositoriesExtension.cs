using Hestia.Domain.Repositories;
using Hestia.Domain.Repositories.Projects;
using Hestia.Domain.Repositories.Users;
using Hestia.Infrastructure.Repositories;
using Hestia.Infrastructure.Repositories.Projects;
using Hestia.Infrastructure.Repositories.Users;

namespace Hestia.API.Extensions;

public static class RepositoriesExtension
{
    public static void AddHestiaRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProjectCategoryRepository, ProjectCategoryRepository>();
        services.AddScoped<IOAuthStateRepository, OAuthStateRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
    }
}