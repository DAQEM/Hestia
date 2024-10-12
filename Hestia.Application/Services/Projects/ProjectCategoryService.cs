using System.Data.Common;
using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Result;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Repositories.Projects;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Projects;

public class ProjectCategoryService(IProjectCategoryRepository projectCategoryRepository, IMapper mapper)
{
    public async Task<IResult<List<ProjectCategoryDto>>> GetAllAsync()
    {
        IEnumerable<ProjectCategory> projectCategories = await projectCategoryRepository.GetAllAsync();
        
        return new ServiceResult<List<ProjectCategoryDto>>
        {
            Data = projectCategories.Select(mapper.Map<ProjectCategoryDto>).ToList(),
            Success = true,
            Message = "Project categories found"
        };
    }
    
    public async Task<IResult<ProjectCategoryDto?>> GetAsync(int id)
    {
        ProjectCategory? projectCategory = await projectCategoryRepository.GetAsync(id);
        
        return projectCategory is not null
            ? new ServiceResult<ProjectCategoryDto>
            {
                Data = mapper.Map<ProjectCategoryDto>(projectCategory),
                Success = true,
                Message = "Project category found"
            }
            : new ServiceResult<ProjectCategoryDto>
            {
                Data = null,
                Success = false,
                Message = "Project category not found"
            };
    }
    
    public async Task<IResult<ProjectCategoryDto>> AddAsync(ProjectCategoryDto projectCategory)
    {
        ProjectCategory mappedProjectCategory = mapper.Map<ProjectCategory>(projectCategory);
        ProjectCategory addedProjectCategory = await projectCategoryRepository.AddAsync(mappedProjectCategory);
        await projectCategoryRepository.SaveChangesAsync();
        
        return new ServiceResult<ProjectCategoryDto>
        {
            Data = mapper.Map<ProjectCategoryDto>(addedProjectCategory),
            Success = true,
            Message = "Project category added"
        };
    }

    public async Task AddRangeAsync(List<ProjectCategoryDto> categories)
    {
        List<ProjectCategory> mappedCategories = categories.Select(mapper.Map<ProjectCategory>).ToList();
        await projectCategoryRepository.AddRangeAsync(mappedCategories);
        await projectCategoryRepository.SaveChangesAsync();
    }

    public async Task SetProjectCategoriesAsync(int projectId, string[] allCategories)
    {
        try
        {
            await projectCategoryRepository.SetProjectCategoriesAsync(projectId, allCategories);
            await projectCategoryRepository.SaveChangesAsync();
        } 
        catch (Exception e)
        {
            if (e.InnerException != null && e.InnerException.GetType().GetProperties().Any(p => p.Name == "SqlState") && 
                (string)e.InnerException.GetType().GetProperty("SqlState")?.GetValue(e.InnerException)! == "23505")
            {
                // Ignore duplicate key error
                return;
            }
            
            throw;
        }
    }
}