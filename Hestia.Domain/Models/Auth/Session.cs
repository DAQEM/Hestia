using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Auth;

public class Session : Model<int>
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime RefreshExpiresAt { get; set; }
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
    public string? OperatingSystem { get; set; }
    public string? Browser { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    
    public User User { get; set; }
}