using Hestia.Domain.Models;

namespace Hestia.Application.Dtos.User;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public static RoleDto FromModel(Role? role)
    {
        if (role is null) return new RoleDto();
        
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
        };
    }

    public Role ToModel()
    {
        return new Role
        {
            Id = Id,
            Name = Name,
            Description = Description,
        };
    }
}