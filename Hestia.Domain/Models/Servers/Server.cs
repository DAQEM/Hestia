using System.ComponentModel.DataAnnotations;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Models.Servers;

public class Server : Model<int>
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; } = null!;
    [Required]
    [StringLength(255)]
    public string Slug { get; set; } = null!;
    [Required]
    [StringLength(255)]
    public string Host { get; set; } = null!;
    [Required]
    public int Port { get; set; } = 25565;
    [Required]
    [StringLength(8)]
    public string Version { get; set; } = null!;
    [Required]
    [StringLength(1024)]
    public string Description { get; set; } = null!;
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    [Required]
    public bool IsPublished { get; set; }
    [Required]
    public bool IsFeatured { get; set; }
    [Required]
    public bool IsWhitelisted { get; set; }
    [Required]
    public bool IsOnline { get; set; }
    [Required]
    public int MaxPlayers { get; set; }
    [Required]
    public int OnlinePlayers { get; set; }
    public int? RamMb { get; set; }
    
    public List<ServerCategory> Categories { get; set; } = [];
    public List<Project> Projects { get; set; } = [];
    public List<User> Users { get; set; } = [];
    
}