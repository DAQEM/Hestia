using System.Security.Claims;
using Hestia.Domain.Extensions;

namespace Hestia.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.NameIdentifier))?.Value;
    }

    public static string? GetName(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.Name))?.Value;
    }
    
    public static string? GetBio(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("Bio"))?.Value;
    }

    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.Email))?.Value;
    }

    public static string? GetImage(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.Uri))?.Value;
    }

    public static string? GetRole(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.Role))?.Value.ToLower();
    }
    
    public static void SetName(this ClaimsPrincipal principal, string name)
    {
        ClaimsIdentity? identity = principal.Identity as ClaimsIdentity;
        identity?.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
        identity?.AddClaim(new Claim(ClaimTypes.Name, name));
    }
    
    public static void SetBio(this ClaimsPrincipal principal, string bio)
    {
        ClaimsIdentity? identity = principal.Identity as ClaimsIdentity;
        identity?.RemoveClaim(identity.FindFirst("Bio"));
        identity?.AddClaim(new Claim("Bio", bio));
    }
}