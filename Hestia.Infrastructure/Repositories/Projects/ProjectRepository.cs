using System.Collections;
using System.Reflection;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Projects;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Algorithms;
using Hestia.Infrastructure.Database;
using Hestia.Infrastructure.Exceptions.Projects;
using Hestia.Infrastructure.Queries.Projects;
using Hestia.Infrastructure.Queries.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories.Projects;

public class ProjectRepository(HestiaDbContext dbContext) : IProjectRepository
{
    public async Task<PagedResult<List<Project>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured,
        string[]? categories, string[]? loaders, string[]? types, ProjectOrder? order, string? user,
        bool creator = false)
    {
        IQueryable<Project> projectsQuery = dbContext.Projects
            .Include(p => p.Categories)
            .Include(p => p.Users)
            .Where(p => p.IsPublished || creator)
            .AsQueryable();

        if (isFeatured is not null)
        {
            projectsQuery = projectsQuery.Where(p => p.IsFeatured == isFeatured);
        }

        if (categories is not null)
        {
            projectsQuery = projectsQuery.Where(p => categories.All(c => p.Categories!.Any(cat => cat.Slug.Equals(c))));
        }

        if (loaders is not null)
        {
            projectsQuery = projectsQuery.Where(p =>
                p.Loaders.HasFlag(Enum.Parse<ProjectLoaders>(string.Join(",", loaders), true)));
        }

        if (query is not null)
        {
            projectsQuery = projectsQuery
                .Where(p => EF.Functions.Like(p.Name, $"%{query}%")
                            || EF.Functions.Like(p.Summary, $"%{query}%")
                            || EF.Functions.Like(p.Description, $"%{query}%"));
        }

        if (types is not null)
        {
            projectsQuery =
                projectsQuery.Where(p => p.Type.HasFlag(Enum.Parse<ProjectType>(string.Join(",", types), true)));
        }

        if (user is not null)
        {
            projectsQuery = projectsQuery.Where(p => p.Users.Any(u => u.Name == user));
        }

        projectsQuery = order switch
        {
            ProjectOrder.Name => projectsQuery.OrderBy(p => p.Name),
            ProjectOrder.CreatedAt => projectsQuery.OrderByDescending(p => p.CreatedAt),
            _ => projectsQuery
        };

        List<Project> projects;

        projectsQuery = creator
            ? ProjectQueries.SelectCreatorProjects(projectsQuery)
            : ProjectQueries.SelectSimpleProjects(projectsQuery);

        if (order == ProjectOrder.Relevance)
        {
            List<Project> projectsList = await projectsQuery.ToListAsync().ConfigureAwait(false);

            projects = projectsList
                .OrderByDescending(p =>
                    string.IsNullOrEmpty(query)
                        ? ProjectRelevanceCalculator.CalculateRelevanceScoreWithoutSearchTerm(p)
                        : ProjectRelevanceCalculator.CalculateRelevanceScore(p, query))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        else if (order == ProjectOrder.Downloads)
        {
            List<Project> projectsList = await projectsQuery.ToListAsync().ConfigureAwait(false);

            projects = projectsList
                .OrderByDescending(p => p.CurseForgeDownloads + p.ModrinthDownloads)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }
        else
        {
            projects = await projectsQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        int totalCount = await projectsQuery.CountAsync().ConfigureAwait(false);

        return new PagedResult<List<Project>>
        {
            Data = projects,
            Success = true,
            Message = "Projects found",
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<Project?> GetAsync(int id)
    {
        return await dbContext.Projects.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<Project?> GetBySlugAsync(string slug, bool users = false)
    {
        IQueryable<Project> projectsQuery = dbContext.Projects.AsQueryable();
        if (users) projectsQuery = projectsQuery.Include(p => p.Users);
        projectsQuery = ProjectQueries.SelectSimpleProjects(projectsQuery);
        return await projectsQuery.FirstOrDefaultAsync(p => p.Slug == slug).ConfigureAwait(false);
    }

    public Task<Project?> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users)
    {
        IQueryable<Project> projectsQuery = dbContext.Projects.AsQueryable();

        if (categories)
        {
            projectsQuery = projectsQuery.Include(p => p.Categories);
        }

        if (users)
        {
            projectsQuery = projectsQuery.Include(p => p.Users);
        }

        return projectsQuery.FirstOrDefaultAsync(p => p.Slug == idOrSlug || p.Id.ToString() == idOrSlug);
    }

    public async Task<Project?> GetByModrinthIdAsync(string modrinthId)
    {
        return await dbContext.Projects.FirstOrDefaultAsync(p => p.ModrinthId == modrinthId).ConfigureAwait(false);
    }

    public async Task<Project?> GetByCurseForgeIdAsync(string curseForgeId)
    {
        return await dbContext.Projects.FirstOrDefaultAsync(p => p.CurseForgeId == curseForgeId).ConfigureAwait(false);
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await dbContext.Projects.ToListAsync().ConfigureAwait(false);
    }

    public async Task<Project> AddAsync(Project entity)
    {
        EntityEntry<Project> entry = await dbContext.Projects.AddAsync(entity).ConfigureAwait(false);
        return entry.Entity;
    }

    public async Task AddUserToProjectAsync(int projectId, int userId)
    {
        Project? project = await dbContext.Projects.FindAsync(projectId);

        if (project is not null)
        {
            User? user = await dbContext.Users.FindAsync(userId);

            if (user is not null)
            {
                project.Users ??= [];
                project.Users.Add(user);
            }
        }
    }

    public async Task<Project?> UpdateBySlugAsync(string slug, Project project)
    {
        Project? existing = await dbContext.Projects.FirstOrDefaultAsync(p => p.Slug == slug);

        if (existing is not null)
        {
            existing.IsPublished = project.IsPublished;
            existing.ShouldSync = project.ShouldSync;
            existing.ModrinthId = project.ModrinthId;
            existing.CurseForgeId = project.CurseForgeId;
        }

        return existing;
    }

    public async Task UpdateModrinthProjectAsync(int id, Project project)
    {
        Project? existing = await dbContext.Projects.FindAsync(id);

        if (existing is not null)
        {
            existing.Name = project.Name;
            existing.Summary = project.Description;
            existing.Description = project.Description;
            existing.Slug = project.Slug;
            existing.ImageUrl = project.ImageUrl;
            existing.BannerUrl = project.BannerUrl;
            existing.GitHubUrl = project.GitHubUrl;
            existing.ModrinthId = project.ModrinthId;
            existing.ModrinthUrl = project.ModrinthUrl;
            existing.ModrinthDownloads = project.ModrinthDownloads;
            existing.Loaders = project.Loaders;
            existing.SyncedAt = project.SyncedAt;
            existing.Type = project.Type;
        }
    }

    public async Task UpdateCurseForgeProjectAsync(int id, Project project)
    {
        Project? existing = await dbContext.Projects.FindAsync(id);

        if (existing is not null)
        {
            existing.CurseForgeUrl = project.CurseForgeUrl;
            existing.CurseForgeDownloads = project.CurseForgeDownloads;
            existing.SyncedAt = project.SyncedAt;
        }
    }

    public Task<bool> DeleteAsync(int id)
    {
        EntityEntry<Project> entity = dbContext.Projects.Remove(new Project { Id = id });
        return Task.FromResult(entity.State == EntityState.Deleted);
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}