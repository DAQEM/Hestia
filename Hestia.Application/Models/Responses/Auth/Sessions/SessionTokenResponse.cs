namespace Hestia.Application.Models.Responses.Auth.Sessions;

public class SessionTokenResponse
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}