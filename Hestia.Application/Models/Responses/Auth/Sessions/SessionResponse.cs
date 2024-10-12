namespace Hestia.Application.Models.Responses.Auth.Sessions;

public class SessionResponse
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string IpAddress { get; set; }
    public string? OperatingSystem { get; set; }
    public string? Browser { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public bool IsCurrentSession { get; set; }
}