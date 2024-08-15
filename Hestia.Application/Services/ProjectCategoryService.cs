using Hestia.Application.Dtos.Project;
using Hestia.Application.Result;
using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Domain.Result;

namespace Hestia.Application.Services;

public class ProjectCategoryService(IProjectCategoryRepository projectCategoryRepository)
{
    public async Task<IResult<List<ProjectCategoryDto>>> GetAllAsync()
    {
        IEnumerable<ProjectCategory> projectCategories = await projectCategoryRepository.GetAllAsync();
        
        return new ServiceResult<List<ProjectCategoryDto>>
        {
            Data = projectCategories.Select(ProjectCategoryDto.FromModel).ToList(),
            Success = true,
            Message = "Project categories found"
        };
    }
}