namespace Hestia.Domain.Models.Users;

public class OAuthState : Model<int>
{
    public string State { get; set; } = null!;
    public string ReturnUri { get; set; } = null!;
    public OAuthProvider Provider { get; set; }
    public int? UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
}