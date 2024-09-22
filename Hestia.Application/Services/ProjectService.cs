using Hestia.Application.Dtos.Projects;
using Hestia.Application.Result;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Repositories;
using Hestia.Domain.Repositories.Projects;
using Hestia.Domain.Result;

namespace Hestia.Application.Services;

public class ProjectService(IProjectRepository projectRepository)
{
    public async Task<PagedResult<List<ProjectDto>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string[]? loaders, string[]? types, ProjectOrder? order, string? user)
    {
        PagedResult<List<Project>> pagedResult = await projectRepository.SearchAsync(query, page, pageSize, isFeatured, categories, loaders, types, order!, user);
        
        return new PagedResult<List<ProjectDto>>
        {
            Data = pagedResult.Data?.Select(ProjectDto.FromModel).ToList() ?? [],
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
        
        return (project is null ? ProjectNotFoundResult! : ProjectFoundResult(project))!;
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetBySlugAsync(string slug)
    {
        Project? project = await projectRepository.GetBySlugAsync(slug);
        
        return (project is null ? ProjectNotFoundResult! : ProjectFoundResult(project))!;
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users)
    {
        Project? project = await projectRepository.GetByIdOrSlugAsync(idOrSlug, categories, users);
        
        return (project is null ? ProjectNotFoundResult! : ProjectFoundResult(project))!;
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetByModrinthIdAsync(string modrinthId)
    {
        Project? project = await projectRepository.GetByModrinthIdAsync(modrinthId);
        
        return (project is null ? ProjectNotFoundResult! : ProjectFoundResult(project))!;
    }
    
    public async Task<ServiceResult<ProjectDto?>> GetByCurseForgeIdAsync(string curseForgeId)
    {
        Project? project = await projectRepository.GetByCurseForgeIdAsync(curseForgeId);
        
        return (project is null ? ProjectNotFoundResult! : ProjectFoundResult(project))!;
    }

    public async Task<ServiceResult<IEnumerable<ProjectDto>>> GetAllAsync()
    {
        IEnumerable<Project> projects = await projectRepository.GetAllAsync();
        
        return new ServiceResult<IEnumerable<ProjectDto>>
        {
            Data = projects.Select(ProjectDto.FromModel),
            Success = true,
            Message = "Project found"
        };
    }

    public async Task<ServiceResult<ProjectDto>> AddAsync(ProjectDto project)
    {
        Project addedProject = await projectRepository.AddAsync(project.ToModel());

        await projectRepository.SaveChangesAsync();
        
        return new ServiceResult<ProjectDto>
        {
            Data = ProjectDto.FromModel(addedProject),
            Success = true,
            Message = "Project added successfully"
        };
    }

    public async Task<ServiceResult<ProjectDto>> UpdateAsync(int id, ProjectDto newProject)
    {
        Project updatedProject = await projectRepository.UpdateAsync(id, newProject.ToModel());

        await projectRepository.SaveChangesAsync();
        
        return new ServiceResult<ProjectDto>
        {
            Data = ProjectDto.FromModel(updatedProject),
            Success = true,
            Message = "Project updated successfully"
        };
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
    
    private static ServiceResult<ProjectDto> ProjectFoundResult(Project project) => new()
    {
        Data = ProjectDto.FromModel(project),
        Success = true,
        Message = "Project found"
    };
}