using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Blogs;

public class Blog : Model<int>
{
    [Required, StringLength(128)]
    public string Name { get; set; }
    
    [Required, StringLength(128)]
    public string Slug { get; set; }
    
    [Required, StringLength(1024)]
    public string Summary { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required, StringLength(1024)]
    public string ImageUrl { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? PublishedAt { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsPublished { get; set; }
    
    
    public List<User>? Users { get; set; }
    public List<BlogComment>? Comments { get; set; }
    public List<BlogCategory>? Categories { get; set; }
    public List<Project>? Projects { get; set; }
}