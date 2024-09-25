using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Projects;

namespace Hestia.Domain.Models.Users;

public class User : Model<int>
{
    public string Name { get; set; } = null!;
    public string? Bio { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    public Role Role { get; set; } = Role.Player;
    public DateTime Joined { get; set; }
    public DateTime LastActive { get; set; }
    
    public long? DiscordId { get; set; }
    
    public List<Blog> Blogs { get; set; } = null!;
    public List<BlogComment> Comments { get; set; } = null!;
    public List<Project> Projects { get; set; } = null!;
    public List<Session> Sessions { get; set; } = null!;
}