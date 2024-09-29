using Hestia.Domain.Repositories.Auth;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Domain.Repositories.Projects;
using Hestia.Domain.Repositories.Users;
using Hestia.Infrastructure.Repositories.Auth;
using Hestia.Infrastructure.Repositories.Blogs;
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
        services.AddScoped<IBlogRepository, BlogRepository>();
    }
}