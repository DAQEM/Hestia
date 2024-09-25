namespace Hestia.Domain.Models.Servers;

public class ServerCategory
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public List<Server> Servers { get; set; } = [];
}