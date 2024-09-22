using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Blogs;

public class Blog : Model<int>
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }
    
    public List<User> Users { get; set; } = null!;
    public List<BlogComment> Comments { get; set; } = [];
    public List<BlogCategory> Categories { get; set; } = [];
    public List<Project> Projects { get; set; } = [];
}