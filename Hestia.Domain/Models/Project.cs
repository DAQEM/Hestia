namespace Hestia.Domain.Models;

public class Project : Model<int>
{
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? BannerUrl { get; set; }
    public long Downloads { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProjectType Type { get; set; }
    public ProjectLoaders Loaders { get; set; }
    
    public List<Post> Posts { get; set; } = [];
    public List<ProjectCategory> Categories { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public List<ProjectVersion> Versions { get; set; } = [];
}