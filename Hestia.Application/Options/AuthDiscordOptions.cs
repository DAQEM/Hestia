namespace Hestia.Application.Options;

public class AuthDiscordOptions
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string Scopes { get; set; } = null!;
    public string Callback { get; set; } = null!;
}