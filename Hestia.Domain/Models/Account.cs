namespace Hestia.Domain.Models;

public class Account : Model<int>
{
    public string AccessToken { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public long ExpiresAt { get; set; }
    public string Scope { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public string ProviderAccountId { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int UserId { get; set; }
    
    public User User { get; set; } = null!;
}