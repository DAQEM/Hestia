using Hestia.Domain.Models.Projects;

namespace Hestia.Domain.Repositories.Projects;

public interface IProjectCategoryRepository : IRepository<ProjectCategory, int>
{
    Task AddRangeAsync(List<ProjectCategory> mappedCategories);
    Task SetProjectCategoriesAsync(int projectId, string[] categories);
}