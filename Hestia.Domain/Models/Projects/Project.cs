using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Servers;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Models.Wikis;

namespace Hestia.Domain.Models.Projects;

public class Project : Model<int>
{
    [Required, StringLength(128)]
    public string Name { get; set; }
    
    [Required, StringLength(1024)]
    public string Summary { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required, StringLength(128)]
    public string Slug { get; set; }
    
    [Required, StringLength(1024)]
    public string ImageUrl { get; set; }
    
    [StringLength(1024)]
    public string? BannerUrl { get; set; }
    
    [StringLength(1024)]
    public string? GitHubUrl { get; set; }
    
    [StringLength(16)]
    public string? CurseForgeId { get; set; }
    
    [StringLength(1024)]
    public string? CurseForgeUrl { get; set; }
    
    public long? CurseForgeDownloads { get; set; }
    
    [StringLength(16)]
    public string? ModrinthId { get; set; }
    
    [StringLength(1024)]
    public string? ModrinthUrl { get; set; }
    
    public long? ModrinthDownloads { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsFeatured { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsPublished { get; set; }
    
    [Required, DefaultValue(true)]
    public bool ShouldSync { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public DateTime SyncedAt { get; set; }
    
    [Required]
    public ProjectType Type { get; set; }
    
    [Required, DefaultValue(ProjectLoaders.None)]
    public ProjectLoaders Loaders { get; set; }
    
    
    public List<Blog>? Blogs { get; set; }
    public List<ProjectCategory>? Categories { get; set; }
    public List<User>? Users { get; set; }
    public List<ProjectVersion>? Versions { get; set; }
    public List<Server>? Servers { get; set; }
    public List<Wiki>? Wikis { get; set; }
}