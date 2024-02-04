using Hestia.Application.Dtos.Project;
using Hestia.Application.Result;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;

namespace Hestia.Application.Services;

public class ProjectService(IProjectRepository projectRepository)
{
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
        try
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
        catch (Exception ex)
        {
            return new ServiceResult<ProjectDto>
            {
                Data = null,
                Success = false,
                Message = $"An error occurred while adding the project: {ex.Message}"
            };
        }
    }

    public async Task<IResult<ProjectDto>> UpdateAsync(int id, ProjectDto newProject)
    {
        try
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
        catch (Exception ex)
        {
            return new ServiceResult<ProjectDto>
            {
                Data = null,
                Success = false,
                Message = $"An error occurred while updating the project: {ex.Message}"
            };
        }
    }

    public async Task<IResult<bool>> DeleteAsync(int id)
    {
        try
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
        catch (Exception ex)
        {
            return new ServiceResult<bool>
            {
                Data = false,
                Success = false,
                Message = $"An error occurred while deleting the project: {ex.Message}"
            };
        }
    }
}