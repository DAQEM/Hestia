using System.Reflection;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Database;
using Hestia.Infrastructure.Exceptions.Blogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories.Blogs;

public class BlogRepository(HestiaDbContext dbContext) : IBlogRepository
{
    public async Task<PagedResult<List<Blog>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string? user)
    {
        IQueryable<Blog> blogsQuery = dbContext.Blogs
            .Include(b => b.Categories)
            .Include(b => b.Users)
            .AsQueryable();

        if (isFeatured is not null)
        {
            blogsQuery = blogsQuery.Where(b => b.IsFeatured == isFeatured);
        }
        
        if (categories is not null)
        {
            blogsQuery = blogsQuery.Where(b => b.Categories.Any(c => categories.Contains(c.Name)));
        }
        
        if (query is not null)
        {
            blogsQuery = blogsQuery
                .Where(b => EF.Functions.Like(b.Name, $"%{query}%") 
                            || EF.Functions.Like(b.Summary, $"%{query}%")
                            || EF.Functions.Like(b.Content, $"%{query}%"));
        }
        
        if (user is not null)
        {
            blogsQuery = blogsQuery.Where(p => p.Users.Any(u => u.Name.Equals(user, StringComparison.CurrentCultureIgnoreCase)));
        }

        List<Blog> blogs = await blogsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync()
            .ConfigureAwait(false);
        
        int totalCount = await blogsQuery.CountAsync().ConfigureAwait(false);
        
        return new PagedResult<List<Blog>>
        {
            Data = blogs,
            Success = true,
            Message = "Projects found",
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };

    }

    public async Task<Blog?> GetAsync(int id)
    {
        return await dbContext.Blogs.FindAsync(id).ConfigureAwait(false);
    }

    public Task<Blog?> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users)
    {
        IQueryable<Blog> blogsQuery = dbContext.Blogs.AsQueryable();
        
        if (categories)
        {
            blogsQuery = blogsQuery.Include(b => b.Categories);
        }
        
        if (users)
        {
            blogsQuery = blogsQuery.Include(b => b.Users);
        }
        
        return blogsQuery.FirstOrDefaultAsync(b => b.Id.ToString() == idOrSlug || b.Slug == idOrSlug);
    }

    public async Task<List<Blog>> GetAllAsync()
    {
        return await dbContext.Blogs.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Blog> AddAsync(Blog entity)
    {
        await dbContext.Blogs.AddAsync(entity).ConfigureAwait(false);
        return entity;
    }

    public async Task<Blog> UpdateAsync(int id, Blog entity)
    {
        Blog? existingBlog = await dbContext.Blogs.FindAsync(id);

        if (existingBlog is null)
        {
            throw new BlogNotFoundException(id);
        }
        
        foreach (PropertyInfo property in typeof(Blog).GetProperties())
        {
            object? newValue = property.GetValue(entity);
            if (newValue != null)
            {
                property.SetValue(existingBlog, newValue);
            }
        }

        return entity;
    }

    public Task<bool> DeleteAsync(int id)
    {
        EntityEntry<Blog> entity = dbContext.Blogs.Remove(new Blog { Id = id });
        return Task.FromResult(entity.State == EntityState.Deleted);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}