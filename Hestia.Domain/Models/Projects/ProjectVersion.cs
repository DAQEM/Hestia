using System.ComponentModel.DataAnnotations;

namespace Hestia.Domain.Models.Projects;

public class ProjectVersion : Model<int>
{
    [Required, StringLength(128)]
    public string Name { get; set; }
    
    [Required, StringLength(128)]
    public string Slug { get; set; }
    
    public List<Project>? Projects { get; set; }
}