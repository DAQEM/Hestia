using System.Text.Json.Serialization;
using Hestia.Domain.Models.Users;

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
    
    public static UserDto FromModel(User? user)
    {
        if (user is null) return new UserDto();
        
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Bio = user.Bio,
            Image = user.Image,
            Role = user.Role,
            Joined = user.Joined,
            LastActive = user.LastActive,
            DiscordId = user.DiscordId
        };
    }

    public User ToModel()
    {
        return new User
        {
            Id = Id,
            Name = Name,
            Bio = Bio,
            Email = Email,
            Image = Image,
            Role = Role,
            Joined = Joined,
            LastActive = LastActive,
            DiscordId = DiscordId
        };
    }
}