using System.Text.Json.Serialization;

namespace Hestia.Application.Dtos.User;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Image { get; set; } = null!;
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RoleDto Role { get; set; } = RoleDto.Player;
    public List<AccountDto>? Accounts { get; set; }

    public static UserDto FromModel(Domain.Models.User? user)
    {
        if (user is null) return new UserDto();
        
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Image = user.Image,
            Role = (RoleDto) user.Role,
            Accounts = user.Accounts?.Select(AccountDto.FromModel).ToList() ?? []
        };
    }

    public Domain.Models.User ToModel()
    {
        return new Domain.Models.User
        {
            Id = Id,
            Name = Name,
            Email = Email,
            Image = Image,
            Role = (Domain.Models.Role) Role,
            Accounts = Accounts?.Select(x => x.ToModel()).ToList() ?? []
        };
    }
}