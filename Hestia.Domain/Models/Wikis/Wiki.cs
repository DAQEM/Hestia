using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Wikis;

public class Wiki : Model<int>
{
    public int? ParentId { get; set; }
    
    [Required]   
    public int ProjectId { get; set; }
    
    [Required, StringLength(128)]
    public string Name { get; set; }
    
    [Required, StringLength(128)]
    public string Slug { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsPublished { get; set; }
    
    
    public Wiki? Parent { get; set; }
    public Project? Project { get; set; }
    public ICollection<Wiki>? Children { get; set; }
    public ICollection<User>? Authors { get; set; }
}