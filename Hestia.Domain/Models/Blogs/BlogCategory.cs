using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hestia.Domain.Models.Blogs;

public class BlogCategory : Model<int>
{
    [ForeignKey(nameof(Parent))]
    public int? ParentId { get; set; }
    
    [Required, StringLength(128)]
    public string Name { get; set; }
    
    [Required, StringLength(128)]
    public string Slug { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required, StringLength(1024)]
    public string ImageUrl { get; set; }
    
    
    public BlogCategory? Parent { get; set; }
    public List<BlogCategory> Children { get; set; } = [];
    public List<Blog> Blogs { get; set; } = [];
}