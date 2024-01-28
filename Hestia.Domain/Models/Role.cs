namespace Hestia.Domain.Models;

public class Role : Model<int>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<User> Users { get; set; } = null!;
}