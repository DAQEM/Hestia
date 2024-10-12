namespace Hestia.Application.Dtos.Blogs;

public class BlogCategoryDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
    public string ImageUrl { get; set; }
    public List<BlogCategoryDto> Children { get; set; } = [];
}