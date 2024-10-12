using System.ComponentModel.DataAnnotations;

namespace Hestia.Domain.Models.Projects;

public class ProjectCategory : Model<int>
{
    [Required, StringLength(128)]
    public string Name { get; set; }
    
    [Required, StringLength(128)]
    public string Slug { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    
    public List<Project>? Projects { get; set; }
}