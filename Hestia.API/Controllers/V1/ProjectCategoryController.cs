using Hestia.Application.Dtos.Project;
using Hestia.Application.Services;
using Hestia.Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]")]
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