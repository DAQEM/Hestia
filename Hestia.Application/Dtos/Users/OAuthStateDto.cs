using Hestia.Domain.Models.Users;

namespace Hestia.Application.Dtos.Users;

public class OAuthStateDto
{
    public int Id { get; set; }
    public string State { get; set; } = null!;
    public string ReturnUri { get; set; } = null!;
    public OAuthProvider Provider { get; set; }
    public int? UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}