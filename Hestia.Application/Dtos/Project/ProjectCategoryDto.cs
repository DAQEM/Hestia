using Hestia.Domain.Models;

namespace Hestia.Application.Dtos.Project;

public class ProjectCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string MetaTitle { get; set; } = null!;
    public string Content { get; set; } = null!;
    
    public static ProjectCategoryDto FromModel(ProjectCategory arg)
    {
        return new ProjectCategoryDto
        {
            Id = arg.Id,
            Name = arg.Name,
            Slug = arg.Slug,
            MetaTitle = arg.MetaTitle,
            Content = arg.Content
        };
    }
}