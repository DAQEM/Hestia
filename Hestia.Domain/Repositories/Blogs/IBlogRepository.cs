using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Result;

namespace Hestia.Domain.Repositories.Blogs;

public interface IBlogRepository : IRepository<Blog, int>
{
    Task<PagedResult<List<Blog>>> SearchAsync(string? query, int page, int pageSize, string[]? categories, string? user, bool creator = false);
    Task<Blog?> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users);
    Task AddUserToBlogAsync(int blogId, int userId);
}