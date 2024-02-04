using Hestia.Application.Dtos.Project;
using Hestia.Application.Result;
using Hestia.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/[controller]")]
public class ProjectController(ProjectService projectService, ILogger<ProjectController> logger)
    : HestiaController(logger)
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        IResult<IEnumerable<ProjectDto>> result = await projectService.GetAllAsync();
        return HandleResult(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProject(int id)
    {
        IResult<ProjectDto?> result = await projectService.GetAsync(id);
        return HandleResult(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProject(ProjectDto project)
    {
        IResult<ProjectDto> result = await projectService.AddAsync(project);
        return HandleResult(result);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectDto project)
    {
        IResult<ProjectDto> result = await projectService.UpdateAsync(id, project);
        return HandleResult(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        IResult<bool> result = await projectService.DeleteAsync(id);
        return result.Success ? Ok() : HandleFailedResult(result);
    }
}