using System.Security.Claims;
using System.Text.Encodings.Web;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Services.Auth;
using Hestia.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hestia.Infrastructure.Handlers;

public class HestiaAuthenticationHandler(
    IOptionsMonitor<HestiaAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock)
    : AuthenticationHandler<HestiaAuthenticationOptions>(options, logger, encoder, clock)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string? token = Request.Headers["Authorization"].FirstOrDefault();
        
        if (token is null || string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
        {
            return AuthenticateResult.NoResult();
        }
        
        token = token["Bearer ".Length..];
        
        if (string.IsNullOrEmpty(token) || token.Split('_').Length != 2)
        {
            return AuthenticateResult.NoResult();
        }
        
        string tokenPrefix = token.Split('_')[0];
        
        UserDto? user = tokenPrefix switch
        {
            "ses" => await Context.RequestServices.GetRequiredService<SessionService>().GetUserByTokenAsync(token),
            "api" => null,
            _ => null
        };
        
        if (user is null)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        List<Claim> claims = [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role.ToString()),
            new(ClaimTypes.Email, user.Email),
        ];
        
        if (user.Bio is not null)
        {
            claims.Add(new Claim("user.bio", user.Bio));
        }
        
        if (user.Image is not null)
        {
            claims.Add(new Claim("user.image", user.Image));
        }

        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        
        AuthenticationTicket ticket = new(principal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);

        
    }
}