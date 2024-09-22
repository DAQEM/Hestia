namespace Hestia.Application.Options;

public class ApplicationOptions
{
    public const string Section = "Application";
    
    public string HestiaUrl { get; set; } = null!;
    public string AsteriaUrl { get; set; } = null!;
    public string OAuthRedirectUrlRegex { get; set; } = null!;
}