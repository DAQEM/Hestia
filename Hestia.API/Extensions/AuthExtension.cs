using Hestia.API.Exceptions;
using Hestia.API.Handlers;
using Hestia.Infrastructure.Events.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Hestia.API.Extensions;

public static class AuthExtension
{
    public static void AddHestiaAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection oAuthSection = configuration.GetSection("OAuth");

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddHestiaCookieAndBearerAuthentication(CookieAuthenticationDefaults.AuthenticationScheme, null, options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    //log comma separated keys
                    Console.WriteLine("Keys: " +
                                      context.Request.Cookies.Keys.Aggregate("",
                                          (current, key) => current + $"{key}, "));
                    return Task.CompletedTask;
                };
            })
            .AddDiscord(options =>
            {
                IConfigurationSection discordSection = oAuthSection.GetSection("Discord");
                string? clientId = discordSection.GetValue<string>("ClientId");
                string? clientSecret = discordSection.GetValue<string>("ClientSecret");

                if (clientId == null)
                {
                    throw new MissingEnvironmentVariableException("DISCORD_CLIENT_ID");
                }

                if (clientSecret == null)
                {
                    throw new MissingEnvironmentVariableException("DISCORD_CLIENT_SECRET");
                }

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
                options.Scope.Add("email");
                options.Events.OnCreatingTicket = OnCreatingTicketEvent.Execute;
            });
    }

    public static void UseHestiaAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    private static AuthenticationBuilder AddHestiaCookieAndBearerAuthentication(this AuthenticationBuilder builder,
        string authenticationScheme, string? displayName, Action<CookieAuthenticationOptions> configureOptions)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor
            .Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
        builder.Services.AddOptions<CookieAuthenticationOptions>(authenticationScheme).Validate(
            o => o.Cookie.Expiration == null, "Cookie.Expiration is ignored, use ExpireTimeSpan instead.");
        return builder.AddScheme<CookieAuthenticationOptions, HestiaAuthenticationHandler>(authenticationScheme,
            displayName, configureOptions);
    }
}