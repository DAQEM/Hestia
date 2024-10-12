namespace Hestia.Application.Models.Responses.Auth.OAuth;

public class OAuthUserResponse
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? ImageUrl { get; set; } = null!;
}