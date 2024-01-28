using System.Security.Claims;
using AspNet.Security.OAuth.Discord;
using Hestia.API.Controllers.V1;
using Hestia.API.Exceptions;
using Hestia.Domain.Models;
using Hestia.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

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

                options.Events.OnCreatingTicket = context =>
                {
                    HestiaDbContext dbContext =
                        context.HttpContext.RequestServices.GetRequiredService<HestiaDbContext>();

                    ClaimsPrincipal? contextPrincipal = context.Principal;
                    string? accountId = contextPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
                    string? name = contextPrincipal?.FindFirstValue(ClaimTypes.Name);
                    string? email = contextPrincipal?.FindFirstValue(ClaimTypes.Email);
                    string? image = contextPrincipal?.FindFirstValue("urn:discord:avatar:hash");

                    string? accessToken = context.AccessToken;
                    string? refreshToken = context.RefreshToken;
                    string? tokenType = context.TokenType;
                    TimeSpan? expiresIn = context.ExpiresIn;

                    if (contextPrincipal == null || accountId == null || name == null || email == null ||
                        image == null || accessToken == null || refreshToken == null || tokenType == null ||
                        expiresIn == null)
                    {
                        context.HttpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }

                    User user = new()
                    {
                        Name = name,
                        Email = email,
                        Image = $"https://cdn.discordapp.com/avatars/{accountId}/{image}",
                        Accounts =
                        [
                            new Account
                            {
                                ProviderAccountId = accountId,
                                Type = "oauth",
                                Provider = "discord",
                                AccessToken = accessToken,
                                RefreshToken = refreshToken,
                                TokenType = tokenType,
                                ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn.Value.Seconds).Ticks,
                                Scope = "identify email",
                            }
                        ]
                    };

                    //check if user exists
                    User? existingUser = dbContext.Users
                        .Include(user => user.Accounts)
                        .FirstOrDefault(u => u.Email == user.Email);

                    if (existingUser != null)
                    {
                        //update user
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Image = user.Image;

                        //replace account with provider discord
                        existingUser.Accounts.RemoveAll(a => a.Provider == "discord");
                        existingUser.Accounts.Add(user.Accounts.First(a => a.Provider == "discord"));

                        dbContext.Users.Update(existingUser);
                    }
                    else
                    {
                        //create user
                        dbContext.Users.Add(user);
                    }

                    dbContext.SaveChanges();

                    return Task.CompletedTask;
                };
            });
    }

    public static void UseHestiaAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}