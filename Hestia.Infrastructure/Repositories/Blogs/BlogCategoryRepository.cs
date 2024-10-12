using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories.Blogs;

public class BlogCategoryRepository(HestiaDbContext dbContext) : IBlogCategoryRepository
{
    public async Task<List<BlogCategory>> GetAllAsync()
    {
        return await dbContext.BlogCategories
            .ToListAsync()
            .ConfigureAwait(false);
    }

    public async Task<BlogCategory?> GetAsync(int id)
    {
        return await dbContext.BlogCategories.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<BlogCategory> AddAsync(BlogCategory entity)
    {
        return (await dbContext.BlogCategories.AddAsync(entity).ConfigureAwait(false)).Entity;
    }

    public Task<bool> DeleteAsync(int id)
    {
        EntityEntry<BlogCategory> entity = dbContext.BlogCategories.Remove(new BlogCategory { Id = id });
        return Task.FromResult(entity.State == EntityState.Deleted);
    }

    public Task SaveChangesAsync()
    {
        return dbContext.SaveChangesAsync();
    }
}