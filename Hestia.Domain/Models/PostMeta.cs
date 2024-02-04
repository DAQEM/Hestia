namespace Hestia.Domain.Models;

public class PostMeta : Model<int>
{
    public int PostId { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }
    
    public Post Post { get; set; }
}