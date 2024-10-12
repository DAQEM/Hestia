using Hestia.Application.Models.Responses.Users;
using Hestia.Domain.Models.Projects;

namespace Hestia.Application.Models.Responses.Projects;

public class SimpleProjectResponse
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Slug { get; set; }
    public string ImageUrl { get; set; }
    public string GitHubUrl { get; set; }
    public string? CurseForgeUrl { get; set; }
    public long? CurseForgeDownloads { get; set; }
    public string? ModrinthUrl { get; set; }
    public long? ModrinthDownloads { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Type { get; set; }
    public string[]? Loaders { get; set; }
    public string[]? Categories { get; set; }
    public List<UserResponse>? Users { get; set; }
}