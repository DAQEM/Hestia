using System.Reflection;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Database;
using Hestia.Infrastructure.Exceptions.Blogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories.Blogs;

public class BlogRepository(HestiaDbContext dbContext) : IBlogRepository
{
    public async Task<PagedResult<List<Blog>>> SearchAsync(string? query, int page, int pageSize, string[]? categories, string? user, bool creator = false)
    {
        IQueryable<Blog> blogsQuery = dbContext.Blogs
            .Include(b => b.Categories)
            .Include(b => b.Users)
            .OrderBy(b => b.PublishedAt)
            .Where(b => b.IsPublished || creator)
            .AsQueryable();
        
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
            blogsQuery = blogsQuery.Where(p => p.Users.Any(u => u.Name == user));
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
    
    public async Task AddUserToBlogAsync(int blogId, int userId)
    {
        Blog? blog = await dbContext.Blogs.FindAsync(blogId);

        if (blog is not null)
        {
            User? user = await dbContext.Users.FindAsync(userId);

            if (user is not null)
            {
                blog.Users ??= [];
                blog.Users.Add(user);
            }
        }
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