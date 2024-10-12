using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Blogs;

public class BlogComment : Model<int>
{
    [Required, ForeignKey(nameof(Blog))]
    public int BlogId { get; set; }
    
    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(Parent))]
    public int? ParentId { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public DateTime UpdatedAt { get; set; }
    
    
    public Blog? Blog { get; set; }
    public User? User { get; set; }
    public BlogComment? Parent { get; set; }
    public List<BlogComment>? Children { get; set; }
}