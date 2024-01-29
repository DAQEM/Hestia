using Hestia.API.Exceptions;
using Hestia.Infrastructure.Events.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            .AddCookie(options =>
            {
                options.LoginPath = "/api/v1/authentication/login/discord";
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
}