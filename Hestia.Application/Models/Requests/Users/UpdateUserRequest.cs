namespace Hestia.Application.Models.Requests.Users;

public class UpdateUserRequest
{
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
}