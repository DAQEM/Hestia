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
}