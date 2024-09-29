using Hestia.Application.Dtos.Projects;
using Hestia.Application.Services.Projects;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1.Projects;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/projects")]
public class ProjectController(ProjectService projectService, ILogger<ProjectController> logger)
    : HestiaController(logger)
{
    [HttpGet]
    public async Task<IActionResult> SearchProjects([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? isFeatured = null, [FromQuery] string[]? categories = null, [FromQuery] string[]? loaders = null, [FromQuery] string[]? types = null, [FromQuery] ProjectOrder order = ProjectOrder.Relevance, [FromQuery] string? user = null)
    {
        
        IResult<List<ProjectDto>> result = await projectService.SearchAsync(query, page, pageSize, isFeatured, categories, loaders, types, order, user);
        return HandleResult(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(string id, [FromQuery] bool categories = false, [FromQuery] bool users = false) 
    {
        IResult<ProjectDto?> result = await projectService.GetByIdOrSlugAsync(id, categories, users);
        return HandleResult(result);
    }
    
    [Authorize(Roles = "administrator")]
    [HttpPost]
    public async Task<IActionResult> AddProject(ProjectDto project)
    {
        IResult<ProjectDto> result = await projectService.AddAsync(project);
        return HandleResult(result);
    }
    
    [Authorize(Roles = "administrator")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectDto project)
    {
        IResult<ProjectDto> result = await projectService.UpdateAsync(id, project);
        return HandleResult(result);
    }
    
    [Authorize(Roles = "administrator")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        IResult<bool> result = await projectService.DeleteAsync(id);
        return result.Success ? Ok() : HandleFailedResult(result);
    }
}