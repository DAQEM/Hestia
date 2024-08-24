using System.Text.Json.Serialization;

namespace Hestia.Application.Dtos.Project.External;

public class CurseForgeProjectResponseDto
{
    [JsonPropertyName("data")]
    public CurseForgeProjectDto Data { get; set; } = null!;
}