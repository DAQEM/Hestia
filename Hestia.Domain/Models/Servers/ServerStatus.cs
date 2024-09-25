namespace Hestia.Domain.Models.Servers;

public class ServerStatus : Model<int>
{
    public int ServerId { get; set; }
    public ServerStatusType Status { get; set; }
    public DateTime Date { get; set; }
    
    public Server Server { get; set; } = null!;
}