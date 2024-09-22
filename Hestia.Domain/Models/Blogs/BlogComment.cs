using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Blogs;

public class BlogComment : Model<int>
{
    public int BlogId { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Blog Blog { get; set; }
    public User User { get; set; }
    public BlogComment? Parent { get; set; }
    public List<BlogComment> Children { get; set; } = [];
}