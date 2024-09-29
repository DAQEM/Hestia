using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Wikis;

public class Wiki : Model<int>
{
    public int ParentId { get; set; }
    public int ProjectId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }
    
    public Wiki? Parent { get; set; }
    public Project Project { get; set; } = null!;
    public ICollection<Wiki> Children { get; set; } = null!;
    public ICollection<User> Authors { get; set; } = null!;
}