using System.Reflection;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Algorithms;
using Hestia.Infrastructure.Database;
using Hestia.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Hestia.Infrastructure.Repositories;

public class ProjectRepository(HestiaDbContext dbContext) : IProjectRepository
{
    public async Task<PagedResult<List<Project>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string[]? loaders, string[]? types, ProjectOrder? order)
    {
        IQueryable<Project> projectsQuery = dbContext.Projects
            .Include(p => p.Categories)
            .Include(p => p.Users)
            .AsQueryable();
        
        if (isFeatured is not null)
        {
            projectsQuery = projectsQuery.Where(p => p.IsFeatured == isFeatured);
        }
        
        if (categories is not null)
        {
            projectsQuery = projectsQuery.Where(p => p.Categories.Any(c => categories.Contains(c.Name)));
        }
        
        if (loaders is not null)
        {
            projectsQuery = projectsQuery.Where(p => p.Loaders.HasFlag(Enum.Parse<ProjectLoaders>(string.Join(",", loaders), true)));
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
            projectsQuery = projectsQuery.Where(p => p.Type.HasFlag(Enum.Parse<ProjectType>(string.Join(",", types), true)));
            
        }
        projectsQuery = order switch
        {
            ProjectOrder.Name => projectsQuery.OrderBy(p => p.Name),
            ProjectOrder.CreatedAt => projectsQuery.OrderByDescending(p => p.CreatedAt),
            _ => projectsQuery
        };


        List<Project> projects;

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

    public async Task<Project?> GetBySlugAsync(string slug)
    {
        return await dbContext.Projects.FirstOrDefaultAsync(p => p.Slug == slug).ConfigureAwait(false);
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
        await dbContext.Projects.AddAsync(entity).ConfigureAwait(false);
        
        return entity;
    }

    public Task<Project> UpdateAsync(int id, Project entity)
    {
        Project? existingProject = dbContext.Projects.Find(id);
        
        if (existingProject is null)
        {
            throw new ProjectNotFoundException(id);
        }
        
        foreach (PropertyInfo property in typeof(Project).GetProperties())
        {
            object? newValue = property.GetValue(entity);
            if (newValue != null)
            {
                property.SetValue(existingProject, newValue);
            }
        }
        
        dbContext.Projects.Update(existingProject);
        
        return Task.FromResult(entity);
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