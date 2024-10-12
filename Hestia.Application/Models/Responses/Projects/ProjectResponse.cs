using Hestia.Application.Models.Responses.Users;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Servers;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Models.Wikis;

namespace Hestia.Application.Models.Responses.Projects;

public class ProjectResponse
{
    public string Name { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public string Slug { get; set; }
    public string ImageUrl { get; set; }
    public string? BannerUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public long? CurseForgeDownloads { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    public long? ModrinthDownloads { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public bool ShouldSync { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime SyncedAt { get; set; }
    public string Type { get; set; }
    public string[]? Loaders { get; set; }
    public string[]? Categories { get; set; }
    public List<UserResponse>? Users { get; set; }
}