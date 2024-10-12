namespace Hestia.Application.Models.Responses.Blogs;

public class SimpleBlogResponse
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}