using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hestia.Domain.Models.Servers;

public class ServerStatus : Model<int>
{
    [Required, ForeignKey(nameof(Server))]
    public int ServerId { get; set; }
    
    [Required, DefaultValue(ServerStatusType.Offline)]
    public ServerStatusType Status { get; set; }
    
    [Required]
    public DateTime Date { get; set; }
    
    
    public Server? Server { get; set; }
}