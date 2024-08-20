using System.Security.Claims;
using Hestia.Application.Dtos.User;
using Hestia.Application.Services;
using Hestia.Infrastructure.Generator;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;

namespace Hestia.Infrastructure.Events.Authentication;

public static class OnCreatingTicketEvent
{
    public static async Task Execute(OAuthCreatingTicketContext context)
    {
        ClaimsPrincipal? principal = context.Principal;
        string? accountId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        string? name = RandomUsernameGenerator.Generate();
        string? email = principal?.FindFirstValue(ClaimTypes.Email);
        string? image = principal?.FindFirstValue("urn:discord:avatar:hash");

        string? accessToken = context.AccessToken;
        string? refreshToken = context.RefreshToken;
        string? tokenType = context.TokenType;
        TimeSpan? expiresIn = context.ExpiresIn;

        if (principal is null || accountId is null || email is null || accessToken is null || refreshToken is null 
            || tokenType is null || expiresIn is null)
        {
            context.HttpContext.Response.StatusCode = 401;
            return;
        }
        
        UserDto user = new()
        {
            Name = name,
            Email = email,
            Image = image is null ? null : $"https://cdn.discordapp.com/avatars/{accountId}/{image}",
            Role = RoleDto.Player,
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

        if (existingUser is not  null)
        {
            //update user
            existingUser.Email = user.Email;
            existingUser.Image = user.Image;
            
            user.Name = existingUser.Name;
            user.Role = existingUser.Role;
            user.Id = existingUser.Id;

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
            UserDto? newUser = (await userService.AddAsync(user)).Data;
            if (newUser is null)
            {
                context.HttpContext.Response.StatusCode = 401;
                return;
            }

            user = newUser;
        }

        ClaimsIdentity? claimsIdentity = (ClaimsIdentity?) principal.Identity;

        if (claimsIdentity is not null)
        {
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.NameIdentifier));
            claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(ClaimTypes.Name));
            
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Name));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString().ToLower()));
            
            if (user.Image is not null)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Uri, user.Image));
            }
        }
    }
}