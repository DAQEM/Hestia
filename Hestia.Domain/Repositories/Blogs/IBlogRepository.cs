using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Result;

namespace Hestia.Domain.Repositories.Blogs;

public interface IBlogRepository : IRepository<Blog, int>
{
    Task<PagedResult<List<Blog>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string? user);
    Task<Blog?> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users);
}