namespace Hestia.Domain.Models;

public class PostComment : Model<int>
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public Post Post { get; set; }
    public User User { get; set; }
    public PostComment? Parent { get; set; }
    public List<PostComment> Children { get; set; } = [];
}