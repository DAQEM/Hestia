namespace Hestia.Domain.Models;

public class PostCategory : Model<int>
{
    public int ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string MetaTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public PostCategory? Parent { get; set; }
    public List<PostCategory> Children { get; set; } = [];
    public List<Post> Posts { get; set; } = [];
}