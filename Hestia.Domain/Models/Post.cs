namespace Hestia.Domain.Models;

public class Post : Model<int>
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string MetaTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public bool IsPublished { get; set; }
    
    public User User { get; set; } = null!;
    public List<PostComment> Comments { get; set; } = [];
    public List<PostMeta> Meta { get; set; } = [];
    public List<PostCategory> Categories { get; set; } = [];
    public List<Project> Projects { get; set; } = [];
}