using Hestia.Application.Dtos.Projects;
using Hestia.Application.Services;
using Hestia.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1.Projects;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/projects/category")]
public class ProjectCategoryController(ILogger<HestiaController> logger, ProjectCategoryService projectCategoryService) : HestiaController(logger)
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjectCategories()
    {
        IResult<List<ProjectCategoryDto>> result = await projectCategoryService.GetAllAsync();
        
        return result.Success
            ? Ok(result.Data)
            : NotFound(result.Message);
    }
}