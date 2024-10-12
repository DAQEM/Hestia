namespace Hestia.Application.Models.Requests.Blogs;

public class CreateBlogCategoryRequest
{
    public int? ParentId { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
}