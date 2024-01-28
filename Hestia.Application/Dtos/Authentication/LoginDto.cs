namespace Hestia.Application.Dtos.Authentication;

public class LoginDto
{
    public string UserNameOrEmail { get; set; } = null!;
    public string Password { get; set; } = null!;
}