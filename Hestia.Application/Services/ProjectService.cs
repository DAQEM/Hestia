using Hestia.Application.Dtos.Project;
using Hestia.Application.Result;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Domain.Result;

namespace Hestia.Application.Services;

public class ProjectService(IProjectRepository projectRepository)
{
    public async Task<IResult<List<ProjectDto>>> SearchAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string[]? loaders, string[]? types, ProjectOrderDto? order)
    {
        PagedResult<List<Project>> pagedResult = await projectRepository.SearchAsync(query, page, pageSize, isFeatured, categories, loaders, types, (ProjectOrder) order!);
        
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
    
    public async Task<IResult<ProjectDto?>> GetAsync(int id)
    {
        Project? project = await projectRepository.GetAsync(id);
        
        if (project is null)
        {
            return new ServiceResult<ProjectDto?>
            {
                Data = null,
                Success = false,
                Message = "Project not found"
            };
        }
        
        return new ServiceResult<ProjectDto?>
        {
            Data = ProjectDto.FromModel(project),
            Success = true,
            Message = "Project found"
        };
    }

    public async Task<IResult<IEnumerable<ProjectDto>>> GetAllAsync()
    {
        IEnumerable<Project> projects = await projectRepository.GetAllAsync();
        
        return new ServiceResult<IEnumerable<ProjectDto>>
        {
            Data = projects.Select(ProjectDto.FromModel),
            Success = true,
            Message = "Project found"
        };
    }

    public async Task<IResult<ProjectDto>> AddAsync(ProjectDto project)
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

    public async Task<IResult<ProjectDto>> UpdateAsync(int id, ProjectDto newProject)
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
}