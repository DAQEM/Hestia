using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Projects;

namespace Hestia.Application.Dtos.Projects;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? BannerUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public long CurseForgeDownloads { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    public long ModrinthDownloads { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public bool ShouldSync { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime SyncedAt { get; set; }
    public string Type { get; set; } = null!;
    public string[] Categories { get; set; } = [];
    public string[] Loaders { get; set; } = [];
    public List<UserDto> Users { get; set; } = [];
}