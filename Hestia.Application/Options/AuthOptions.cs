namespace Hestia.Application.Options;

public class AuthOptions
{
    public const string Section = "Auth";
    
    public AuthCookieOptions Cookie { get; set; } = null!;
    public AuthDiscordOptions Discord { get; set; } = null!;
}