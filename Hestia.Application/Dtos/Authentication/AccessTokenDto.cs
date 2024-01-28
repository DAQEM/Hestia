namespace Hestia.Application.Dtos.Authentication;

public class AccessTokenDto
{
    public string TokenType { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = null!;
}