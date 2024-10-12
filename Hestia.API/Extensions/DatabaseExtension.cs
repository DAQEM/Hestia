using Hestia.API.Exceptions;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.API.Extensions;

public static class DatabaseExtension
{
    public static void AddHestiaDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration["Database:ConnectionString"];
        
        if (string.IsNullOrEmpty(connectionString)) 
        {
            throw new MissingEnvironmentVariableException("Database:ConnectionString");
        }
        
        services.AddDbContext<HestiaDbContext>(options =>
        {
            options.UseNpgsql(connectionString, builder =>
            {
                builder.MigrationsAssembly("Hestia.API");
            });
        });
    }

    public static void UseHestiaDatabase(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        HestiaDbContext database = scope.ServiceProvider.GetRequiredService<HestiaDbContext>();
        if (database.Database.GetPendingMigrations().Any())
        {
            database.Database.Migrate();
        }
    }
}