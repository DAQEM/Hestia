using Hestia.Domain.Models.Projects;
using Hestia.Infrastructure.Queries.Users;

namespace Hestia.Infrastructure.Queries.Projects;

public static class ProjectQueries
{
    public static IQueryable<Project> SelectCreatorProjects(IQueryable<Project> projects)
    {
        return projects.Select(p => new Project
        {
            Name = p.Name,
            Summary = p.Summary,
            Slug = p.Slug,
            ImageUrl = p.ImageUrl,
            GitHubUrl = p.GitHubUrl,
            CurseForgeId = p.CurseForgeId,
            CurseForgeUrl = p.CurseForgeUrl,
            CurseForgeDownloads = p.CurseForgeDownloads,
            ModrinthId = p.ModrinthId,
            ModrinthUrl = p.ModrinthUrl,
            ModrinthDownloads = p.ModrinthDownloads,
            IsFeatured = p.IsFeatured,
            IsPublished = p.IsPublished,
            ShouldSync = p.ShouldSync,
            CreatedAt = p.CreatedAt,
            SyncedAt = p.SyncedAt,
            Type = p.Type,
            Categories = ProjectCategoryQueries.SelectSimpleCategories(p.Categories),
            Loaders = p.Loaders,
            Users = UserQueries.SelectSimpleUsers(p.Users)
        });
    }
    
    public static IQueryable<Project> SelectSimpleProjects(IQueryable<Project> projects)
    {
        return projects.Select(p => new Project
        {
            Name = p.Name,
            Summary = p.Summary,
            Slug = p.Slug,
            ImageUrl = p.ImageUrl,
            GitHubUrl = p.GitHubUrl,
            CurseForgeUrl = p.CurseForgeUrl,
            CurseForgeDownloads = p.CurseForgeDownloads,
            ModrinthUrl = p.ModrinthUrl,
            ModrinthDownloads = p.ModrinthDownloads,
            CreatedAt = p.CreatedAt,
            Type = p.Type,
            Categories = ProjectCategoryQueries.SelectSimpleCategories(p.Categories),
            Loaders = p.Loaders,
            Users = UserQueries.SelectSimpleUsers(p.Users)
        });
    }
}