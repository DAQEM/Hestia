using Hestia.Domain.Models.Auth;

namespace Hestia.Application.Dtos.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    public Role Role { get; set; }
    public DateTime Joined { get; set; }
    public DateTime LastActive { get; set; }
    
    public long? DiscordId { get; set; }
}