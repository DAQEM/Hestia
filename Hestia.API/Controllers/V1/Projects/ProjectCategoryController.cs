using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Models.Requests;
using Hestia.Application.Models.Requests.Projects;
using Hestia.Application.Services.Projects;
using Hestia.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1.Projects;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/projects/categories")]
public class ProjectCategoryController(ILogger<HestiaController> logger, IMapper mapper, ProjectCategoryService projectCategoryService) : HestiaController(logger)
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjectCategories()
    {
        IResult<List<ProjectCategoryDto>> result = await projectCategoryService.GetAllAsync();
        return HandleResult(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectCategory(int id)
    {
        IResult<ProjectCategoryDto?> result = await projectCategoryService.GetAsync(id);
        return HandleResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProjectCategory(CreateProjectCategoryRequest request)
    {
        ProjectCategoryDto projectCategory = mapper.Map<ProjectCategoryDto>(request);
        IResult<ProjectCategoryDto> result = await projectCategoryService.AddAsync(projectCategory);
        return HandleResult(result);
    }
}