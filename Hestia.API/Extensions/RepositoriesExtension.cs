using Hestia.Domain.Repositories;
using Hestia.Infrastructure.Repositories;

namespace Hestia.API.Extensions;

public static class RepositoriesExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}