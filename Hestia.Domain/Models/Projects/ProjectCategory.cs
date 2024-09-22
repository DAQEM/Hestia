namespace Hestia.Domain.Models.Projects;

public class ProjectCategory : Model<int>
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string MetaTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public List<Project> Projects { get; set; } = [];
}