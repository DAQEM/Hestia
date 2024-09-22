namespace Hestia.Domain.Models.Projects;

public class ProjectVersion : Model<int>
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    
    public List<Project> Projects { get; set; } = [];
}