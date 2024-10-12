using Hestia.Domain.Models.Auth;

namespace Hestia.Application.Models.Responses.Users;

public class UserResponse
{
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public string? Image { get; set; }
    public Roles Roles { get; set; }
    public DateTime Joined { get; set; }
}