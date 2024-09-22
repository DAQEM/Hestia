using Microsoft.AspNetCore.Authentication;

namespace Hestia.Infrastructure.Options;

public class HestiaAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "HestiaAuthenticationScheme";
}