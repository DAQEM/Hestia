using System.ComponentModel;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Servers;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Models.Wikis;

namespace Hestia.Domain.Models.Projects;

public class Project : Model<int>
{
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? BannerUrl { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    [DefaultValue(0L)]
    public long CurseForgeDownloads { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    [DefaultValue(0L)]
    public long ModrinthDownloads { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; }
    [DefaultValue(true)]
    public bool ShouldSync { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime SyncedAt { get; set; }
    public ProjectType Type { get; set; }
    public ProjectLoaders Loaders { get; set; }
    
    public List<Blog> Blogs { get; set; } = [];
    public List<ProjectCategory> Categories { get; set; } = [];
    public List<User> Users { get; set; } = [];
    public List<ProjectVersion> Versions { get; set; } = [];
    public List<Server> Servers { get; set; } = [];
    public List<Wiki> Wikis { get; set; } = [];
}