using Hestia.Domain.Models.Projects;
using Hestia.Domain.Repositories.Projects;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories.Projects;

public class ProjectCategoryRepository(HestiaDbContext dbContext) : IProjectCategoryRepository
{
    public async Task<ProjectCategory?> GetAsync(int id)
    {
        return await dbContext.ProjectCategories.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<List<ProjectCategory>> GetAllAsync()
    {
        return await dbContext.ProjectCategories.ToListAsync().ConfigureAwait(false);
    }

    public async Task<ProjectCategory> AddAsync(ProjectCategory entity)
    {
        return (await dbContext.ProjectCategories.AddAsync(entity).ConfigureAwait(false)).Entity;
    }

    public Task<bool> DeleteAsync(int id)
    {
        EntityEntry<ProjectCategory> entity = dbContext.ProjectCategories.Remove(new ProjectCategory { Id = id });
        return Task.FromResult(entity.State == EntityState.Deleted);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public Task AddRangeAsync(List<ProjectCategory> mappedCategories)
    {
        return dbContext.ProjectCategories.AddRangeAsync(mappedCategories);
    }

    public async Task SetProjectCategoriesAsync(int projectId, string[] categories)
    {
        Project? project = await dbContext.Projects
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == projectId);
    
        if (project is not null)
        {
            // Fetch the new categories to be added
            List<ProjectCategory> newCategories = await dbContext.ProjectCategories
                .Where(category => categories.Contains(category.Slug))
                .ToListAsync();
        
            // Remove categories that are not in the new list
            project.Categories!.RemoveAll(c => !categories.Contains(c.Slug));
        
            // Add new categories that are not already in the project's categories
            foreach (ProjectCategory newCategory in newCategories.Where(newCategory => project.Categories.All(c => c.Slug != newCategory.Slug)))
            {
                project.Categories.Add(newCategory);
            }
        }
    }
}