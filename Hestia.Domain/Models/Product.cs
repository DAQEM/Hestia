namespace Hestia.Domain.Models;

public class Product : Model<int>
{
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public long Downloads { get; set; }
    public string? GitHubUrl { get; set; }
    public string? CurseForgeUrl { get; set; }
    public string? ModrinthUrl { get; set; }
}