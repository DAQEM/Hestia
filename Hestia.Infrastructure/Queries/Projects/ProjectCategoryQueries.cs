using Hestia.Domain.Models.Projects;

namespace Hestia.Infrastructure.Queries.Projects;

public static class ProjectCategoryQueries
{
    public static List<ProjectCategory> SelectSimpleCategories(List<ProjectCategory> categories)
    {
        return categories.Select(c => new ProjectCategory
        {
            Slug = c.Slug
        }).ToList();
    }
}