using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Servers;
using Hestia.Domain.Models.Wikis;

namespace Hestia.Domain.Models.Users;

public class User : Model<int>
{
    [Required, StringLength(24)]
    public string Name { get; set; }
    
    [StringLength(128)]
    public string? Bio { get; set; }
    
    [Required, StringLength(320)]
    public string Email { get; set; }
    
    [Required, StringLength(1024)]
    public string Image { get; set; }
    
    [Required, DefaultValue(Auth.Roles.None)]
    public Roles Roles { get; set; }
    
    [Required]
    public DateTime Joined { get; set; }
    
    [Required]
    public DateTime LastActive { get; set; }
    
    [Required]
    public long DiscordId { get; set; }
    
    
    public List<Blog>? Blogs { get; set; }
    public List<BlogComment>? Comments { get; set; }
    public List<Project>? Projects { get; set; }
    public List<Session>? Sessions { get; set; }
    public List<Server>? Servers { get; set; }
    public List<Wiki>? Wikis { get; set; }
}