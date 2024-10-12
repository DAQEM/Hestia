using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Servers;

public class Server : Model<int>
{
    [Required, StringLength(255)]
    public string Name { get; set; }

    [Required, StringLength(255)]
    public string Slug { get; set; }
    
    [Required, StringLength(255)]
    public string Host { get; set; }
    
    [Required, DefaultValue(25565)]
    public int Port { get; set; }
    
    [Required, StringLength(8)]
    public string Version { get; set; }
    
    [Required, StringLength(1024)]
    public string Description { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsPublished { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsFeatured { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsWhitelisted { get; set; }
    
    [Required, DefaultValue(false)]
    public bool IsOnline { get; set; }
    
    [Required, DefaultValue(0)]
    public int MaxPlayers { get; set; }
    
    [Required, DefaultValue(0)]
    public int OnlinePlayers { get; set; }
    
    public int? RamMb { get; set; }
    
    
    public List<Project>? Projects { get; set; }
    public List<User>? Users { get; set; }
    
}