using System.Text.Json.Serialization;

namespace Hestia.Application.Models.Requests.Projects;

public class UpdateProjectRequest
{
    [JsonPropertyName("is_published")]
    public bool IsPublished { get; set; }
    
    [JsonPropertyName("should_sync")]
    public bool ShouldSync { get; set; }
    
    [JsonPropertyName("modrinth_id")]
    public string? ModrinthId { get; set; }
    
    [JsonPropertyName("curse_forge_id")]
    public string? CurseForgeId { get; set; }
}