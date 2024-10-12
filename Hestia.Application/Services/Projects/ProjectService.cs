using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Models.Requests.Projects;
using Hestia.Application.Result;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Repositories.Projects;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Projects;

public class ProjectService(IProjectRepository projectRepository, IMapper mapper)
{
    public async Task<PagedResult<List<ProjectDto>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string[]? loaders, string[]? types, ProjectOrder? order, string? user, bool creator = false)
    {
        PagedResult<List<Project>> pagedResult = await projectRepository.SearchAsync(query, page, pageSize, isFeatured, categories, loaders, types, order!, user, creator);
        
        return new PagedResult<List<ProjectDto>>
        {
            Data = pagedResult.Data?.Select(mapper.Map<ProjectDto>).ToList() ?? [],
            Success = true,
            Message = "Projects found",
            Page = page,
            PageSize = pageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages
        };
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetAsync(int id)
    {
        Project? project = await projectRepository.GetAsync(id);
        
        return (project is null ? ProjectNotFoundResult : ProjectFoundResult(project));
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetBySlugAsync(string slug, bool users = false)
    {
        Project? project = await projectRepository.GetBySlugAsync(slug, users);
        
        return (project is null ? ProjectNotFoundResult : ProjectFoundResult(project));
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users)
    {
        Project? project = await projectRepository.GetByIdOrSlugAsync(idOrSlug, categories, users);
        
        return (project is null ? ProjectNotFoundResult : ProjectFoundResult(project));
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetByModrinthIdAsync(string modrinthId)
    {
        Project? project = await projectRepository.GetByModrinthIdAsync(modrinthId);
        
        return (project is null ? ProjectNotFoundResult : ProjectFoundResult(project));
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetByCurseForgeIdAsync(string curseForgeId)
    {
        Project? project = await projectRepository.GetByCurseForgeIdAsync(curseForgeId);
        
        return (project is null ? ProjectNotFoundResult : ProjectFoundResult(project));
    }

    public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetAllAsync()
    {
        IEnumerable<Project> projects = await projectRepository.GetAllAsync();
        
        return new ServiceResult<IEnumerable<ProjectDto>>
        {
            Data = projects.Select(mapper.Map<ProjectDto>),
            Success = true,
            Message = "Project found"
        };
    }

    public async Task<ServiceResult<ProjectDto>> AddAsync(ProjectDto project)
    {
        Project addedProject = await projectRepository.AddAsync(mapper.Map<Project>(project));

        await projectRepository.SaveChangesAsync();
        
        return new ServiceResult<ProjectDto>
        {
            Data = mapper.Map<ProjectDto>(addedProject),
            Success = true,
            Message = "Project added successfully"
        };
    }

    public async Task UpdateModrinthProjectAsync(int id, ProjectDto project)
    {
        await projectRepository.UpdateModrinthProjectAsync(id, mapper.Map<Project>(project));
        await projectRepository.SaveChangesAsync();
    }

    public async Task UpdateCurseForgeProjectAsync(int id, ProjectDto project)
    {
        await projectRepository.UpdateCurseForgeProjectAsync(id, mapper.Map<Project>(project));
        await projectRepository.SaveChangesAsync();
    }

    public async Task<IResult<bool>> DeleteAsync(int id)
    {
        bool deleted = await projectRepository.DeleteAsync(id);
        
        if (!deleted)
        {
            return new ServiceResult<bool>
            {
                Data = false,
                Success = false,
                Message = "Project not deleted"
            };
        }
        
        await projectRepository.SaveChangesAsync();
        
        return new ServiceResult<bool>
        {
            Data = true,
            Success = true,
            Message = "Project deleted successfully"
        };
    }

    private static ServiceResult<ProjectDto?> ProjectNotFoundResult => new()
    {
        Data = null,
        Success = false,
        Message = "Project not found"
    };

    private ServiceResult<ProjectDto?> ProjectFoundResult(Project project) => new()
    {
        Data = mapper.Map<ProjectDto>(project),
        Success = true,
        Message = "Project found"
    };

    public async Task AddUserToProjectAsync(int projectId, int userId)
    {
        await projectRepository.AddUserToProjectAsync(projectId, userId);
        await projectRepository.SaveChangesAsync();
    }

    public async Task<IResult<ProjectDto?>> UpdateAsync(string slug, ProjectDto project)
    {
        Project? newProject = await projectRepository.UpdateBySlugAsync(slug, mapper.Map<Project>(project));
        await projectRepository.SaveChangesAsync();

        if (newProject is null) return ProjectNotFoundResult;
        
        return new ServiceResult<ProjectDto>
        {
            Data = mapper.Map<ProjectDto>(newProject),
            Success = true,
            Message = "Project updated successfully"
        };
    }
}