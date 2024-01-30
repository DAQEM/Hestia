using System.Security.Claims;

namespace Hestia.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetId(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.NameIdentifier))?.Value;
    }

    public static string? GetUserName(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("username"))?.Value;
    }

    public static string? GetEmail(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase(ClaimTypes.Email))?.Value;
    }

    public static string? GetImage(this ClaimsPrincipal principal)
    {
        return principal.Claims.FirstOrDefault(x => x.Type.EqualsIgnoreCase("image"))?.Value;
    }
}