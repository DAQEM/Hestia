namespace Hestia.Domain.Models;

public class ProjectCategory : Model<int>
{
    public int ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string MetaTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public ProjectCategory? Parent { get; set; }
    public List<ProjectCategory> Children { get; set; } = [];
    public List<Project> Projects { get; set; } = [];
}