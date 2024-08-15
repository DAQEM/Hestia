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
}