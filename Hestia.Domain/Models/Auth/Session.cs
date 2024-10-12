using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Auth;

public class Session : Model<int>
{
    [Required, StringLength(64)]
    public string Token { get; set; }
    
    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public DateTime LastUsedAt { get; set; }
    
    [Required]
    public DateTime ExpiresAt { get; set; }
    
    [Required]
    public DateTime RefreshExpiresAt { get; set; }
    
    [Required]
    public string UserAgent { get; set; }
    
    [Required, StringLength(64)]
    public string IpAddress { get; set; }
    
    public string? OperatingSystem { get; set; }
    
    public string? Browser { get; set; }
    
    public string? Country { get; set; }
    
    public string? City { get; set; }
    
    
    public User? User { get; set; }
}