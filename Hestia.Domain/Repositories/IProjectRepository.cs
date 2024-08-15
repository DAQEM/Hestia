using Hestia.Domain.Models;
using Hestia.Domain.Result;

namespace Hestia.Domain.Repositories;

public interface IProjectRepository : IRepository<Project, int>
{
    Task<PagedResult<List<Project>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string[]? loaders, string[]? types, ProjectOrder? order);
}