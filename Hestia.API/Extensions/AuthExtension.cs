using Hestia.Infrastructure.Handlers;
using Hestia.Infrastructure.Options;

namespace Hestia.API.Extensions;

public static class AuthExtension
{
    public static void AddHestiaAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection oAuthSection = configuration.GetSection("OAuth");

        services.AddAuthentication
                (HestiaAuthenticationOptions.DefaultScheme)
            .AddScheme<HestiaAuthenticationOptions, HestiaAuthenticationHandler>
            (HestiaAuthenticationOptions.DefaultScheme,
                options =>
                {
                    
                });
    }

    public static void UseHestiaAuthentication(this IApplicationBuilder app)
    {
        app.UseCookiePolicy(new CookiePolicyOptions
        {
            Secure = CookieSecurePolicy.Always
        });
        
        app.UseAuthentication();
        app.UseAuthorization();
    }
}