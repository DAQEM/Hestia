using System.Security.Claims;
using Hestia.Application.Dtos.User;
using Hestia.Application.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace Hestia.Infrastructure.Events.Authentication;

public static class OnCreatingTicketEvent
{
    public static async Task Execute(OAuthCreatingTicketContext context)
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
        
        contextPrincipal.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim("image", user.Image)
        }));
    }
}