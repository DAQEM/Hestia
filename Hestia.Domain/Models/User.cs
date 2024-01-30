using System.Text.Json.Serialization;

namespace Hestia.Domain.Models;

public class User : Model<int>
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Image { get; set; } = null!;
    public Role Role { get; set; } = Role.Player;
    
    public List<Account> Accounts { get; set; } = null!;
}