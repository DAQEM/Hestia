using System.Text.Json.Serialization;

namespace Hestia.Application.Dtos.Projects.External;

public class CurseForgeProjectLinksDto
{
    [JsonPropertyName("websiteUrl")]
    public string WebsiteUrl { get; set; } = null!;
    [JsonPropertyName("wikiUrl")]
    public string WikiUrl { get; set; } = null!;
    [JsonPropertyName("issuesUrl")]
    public string IssuesUrl { get; set; } = null!;
    [JsonPropertyName("sourceUrl")]
    public string SourceUrl { get; set; } = null!;
}