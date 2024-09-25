using Hestia.Domain.Models.Auth;

namespace Hestia.Application.Dtos.Auth;

public class OAuthStateDto
{
    public int Id { get; set; }
    public string State { get; set; } = null!;
    public string ReturnUri { get; set; } = null!;
    public OAuthProvider Provider { get; set; }
    public int? UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}