namespace Hestia.Application.Dtos.User;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Image { get; set; } = null!;
    public List<RoleDto>? Roles { get; set; }
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
            Roles = user.Roles?.Select(RoleDto.FromModel).ToList() ?? [],
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
            Roles = Roles?.Select(x => x.ToModel()).ToList() ?? [],
            Accounts = Accounts?.Select(x => x.ToModel()).ToList() ?? []
        };
    }
}