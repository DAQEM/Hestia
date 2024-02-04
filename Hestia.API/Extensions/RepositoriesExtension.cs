using Hestia.Domain.Repositories;
using Hestia.Infrastructure.Repositories;

namespace Hestia.API.Extensions;

public static class RepositoriesExtension
{
    public static void AddHestiaRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}