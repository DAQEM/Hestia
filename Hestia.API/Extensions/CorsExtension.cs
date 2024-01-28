namespace Hestia.API.Extensions;

public static class CorsExtension
{
    public static void AddHestiaCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
    
    public static void UseHestiaCors(this IApplicationBuilder app)
    {
        app.UseCors("AllowAll");
    }
}