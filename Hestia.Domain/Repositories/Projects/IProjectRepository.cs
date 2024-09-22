using Hestia.Domain.Models.Projects;
using Hestia.Domain.Result;

namespace Hestia.Domain.Repositories.Projects;

public interface IProjectRepository : IRepository<Project, int>
{
    Task<Project?> GetBySlugAsync(string slug);
    Task<Project?> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users);
    Task<Project?> GetByModrinthIdAsync(string modrinthId);
    Task<Project?> GetByCurseForgeIdAsync(string curseForgeId);
    Task<PagedResult<List<Project>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string[]? loaders, string[]? types, ProjectOrder? order, string? user);
}