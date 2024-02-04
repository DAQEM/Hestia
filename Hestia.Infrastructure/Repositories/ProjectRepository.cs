using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories;

public class ProjectRepository(HestiaDbContext dbContext) : IProjectRepository
{
    public async Task<Project?> GetAsync(int id)
    {
        return await dbContext.Projects.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await dbContext.Projects.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Project> AddAsync(Project entity)
    {
        await dbContext.Projects.AddAsync(entity).ConfigureAwait(false);
        
        return entity;
    }

    public Task<Project> UpdateAsync(int id, Project entity)
    {
        dbContext.Projects.Update(entity);
        return Task.FromResult(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Project? project = await dbContext.Projects.FindAsync(id).ConfigureAwait(false);
        if (project is null) return false;
        dbContext.Projects.Remove(project);
        return true;
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}