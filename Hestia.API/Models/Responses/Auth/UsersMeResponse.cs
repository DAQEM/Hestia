namespace Hestia.API.Models.Responses.Auth;

public class UsersMeResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Image { get; set; }
    public string Role { get; set; } = null!;
}