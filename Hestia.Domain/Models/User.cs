namespace Hestia.Domain.Models;

public class User : Model<int>
{
    public string Name { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    public Role Role { get; set; } = Role.Player;
    public long Joined { get; set; }
    public long LastActive { get; set; }
    
    public List<Account> Accounts { get; set; } = null!;
    public List<Post> Posts { get; set; } = null!;
    public List<PostComment> Comments { get; set; } = null!;
}