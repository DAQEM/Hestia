using System.Security.Claims;
using Hestia.API.Exceptions;
using Hestia.Application.Dtos.User;
using Hestia.Application.Services;
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

                options.Events.OnCreatingTicket = async context =>
                {
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
                        return;
                    }

                    UserDto user = new()
                    {
                        Name = name,
                        Email = email,
                        Image = $"https://cdn.discordapp.com/avatars/{accountId}/{image}",
                        Accounts =
                        [
                            new AccountDto
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

                    UserService userService =
                        context.HttpContext.RequestServices.GetRequiredService<UserService>();
                    
                    //check if user exists
                    UserDto? existingUser = (await userService.GetUserWithAccountByEmailAsync(email)).Data;

                    if (existingUser != null)
                    {
                        //update user
                        existingUser.Name = user.Name;
                        existingUser.Email = user.Email;
                        existingUser.Image = user.Image;

                        List<AccountDto> existingAccounts = existingUser.Accounts!;
                        
                        //replace account with provider discord
                        AccountDto? existingAccount = existingAccounts.FirstOrDefault(x =>
                            x.Provider == "discord" && x.ProviderAccountId == accountId);
                        
                        if (existingAccount != null)
                        {
                            existingAccount.AccessToken = accessToken;
                            existingAccount.RefreshToken = refreshToken;
                            existingAccount.TokenType = tokenType;
                            existingAccount.ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn.Value.Seconds).Ticks;
                        }
                        else
                        {
                            existingAccounts.Add(user.Accounts![0]);
                        }

                        await userService.UpdateAsync(existingUser.Id, existingUser);
                    }
                    else
                    {
                        //create user
                        await userService.AddAsync(user);
                    }
                };
            });
    }

    public static void UseHestiaAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}