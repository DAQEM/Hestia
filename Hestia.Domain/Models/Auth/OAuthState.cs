using System.ComponentModel.DataAnnotations;

namespace Hestia.Domain.Models.Auth;

public class OAuthState : Model<int>
{
    [Required, StringLength(32)]
    public string State { get; set; }
    
    [Required, StringLength(2048)]
    public string ReturnUri { get; set; }
    
    [Required]
    public OAuthProvider Provider { get; set; }
    
    public int? UserId { get; set; }
    
    [Required]
    public DateTime ExpiresAt { get; set; }
}