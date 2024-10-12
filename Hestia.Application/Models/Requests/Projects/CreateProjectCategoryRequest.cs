namespace Hestia.Application.Models.Requests.Projects;

public class CreateProjectCategoryRequest
{
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Content { get; set; } = null!;
}