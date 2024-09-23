namespace Hestia.API.Models.Responses.Auth;

public class SessionTokenResponse
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}