namespace Hestia.Domain.Models.Blogs;

public class BlogCategory : Model<int>
{
    public int ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    
    public BlogCategory? Parent { get; set; }
    public List<BlogCategory> Children { get; set; } = [];
    public List<Blog> Blogs { get; set; } = [];
}