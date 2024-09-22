namespace Hestia.Application.Options;

public class ApiKeyOptions
{
    public const string Section = "ApiKeys";
    
    public string Modrinth { get; set; } = null!;
    public string CurseForge { get; set; } = null!;
}