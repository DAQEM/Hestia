using System.Text.Json.Serialization;
using Hestia.Application.Converters.Json;

namespace Hestia.Application.Dtos.User;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    
    [JsonConverter(typeof(LowercaseStringEnumConverter<RoleDto>))]
    public RoleDto Role { get; set; }
    public long Joined { get; set; }
    public long LastActive { get; set; }
    public List<AccountDto>? Accounts { get; set; }

    public static UserDto FromModel(Domain.Models.User? user)
    {
        if (user is null) return new UserDto();
        
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Bio = user.Bio,
            Email = user.Email,
            Image = user.Image,
            Role = (RoleDto) user.Role,
            Joined = user.Joined,
            LastActive = user.LastActive,
            Accounts = user.Accounts?.Select(AccountDto.FromModel).ToList() ?? []
        };
    }

    public Domain.Models.User ToModel()
    {
        return new Domain.Models.User
        {
            Id = Id,
            Name = Name,
            Bio = Bio,
            Email = Email,
            Image = Image,
            Role = (Domain.Models.Role) Role,
            Joined = Joined,
            LastActive = LastActive,
            Accounts = Accounts?.Select(x => x.ToModel()).ToList() ?? []
        };
    }
}