using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories;

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

    public Task<ProjectCategory> UpdateAsync(int id, ProjectCategory entity)
    {
        return Task.FromResult(dbContext.ProjectCategories.Update(entity).Entity);
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
}