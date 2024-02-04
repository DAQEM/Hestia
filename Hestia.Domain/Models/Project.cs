namespace Hestia.Domain.Models;

public class Project : Model<int>
{
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public long Downloads { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeId { get; set; }
    public string? CurseForgeUrl { get; set; }
    public string? ModrinthId { get; set; }
    public string? ModrinthUrl { get; set; }
    
    public List<Post> Posts { get; set; } = [];
}