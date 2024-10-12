using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Models.Requests.Projects;
using Hestia.Application.Models.Responses.Projects;
using Hestia.Application.Options;
using Hestia.Application.Result;
using Hestia.Application.Services.Projects;
using Hestia.Domain.Extensions;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Hestia.API.Controllers.V1.Projects;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/projects")]
public class ProjectController(ProjectService projectService, IMapper mapper, ILogger<ProjectController> logger)
    : HestiaController(logger)
{
    [HttpGet]
    [ResponseCache(Duration = 30)]
    public async Task<IActionResult> SearchProjects([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? isFeatured = null, [FromQuery] string[]? categories = null, [FromQuery] string[]? loaders = null, [FromQuery] string[]? types = null, [FromQuery] ProjectOrder order = ProjectOrder.Relevance, [FromQuery] string? user = null)
    {
        IResult<List<ProjectDto>> result = await projectService.SearchAsync(query, page, pageSize, isFeatured, categories, loaders, types, order, user);
        if (result.Data is null) return NotFound();
        if (result.Success is false) return HandleFailedResult(result);
        
        return HandleResult(mapper.Map<PagedResult<List<SimpleProjectResponse>>>(result));
    }
    
    [HttpGet("user")]
    [ResponseCache(Duration = 30)]
    public async Task<IActionResult> SearchUserProjects([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string[]? types = null, [FromQuery] ProjectOrder order = ProjectOrder.Relevance)
    {
        IResult<List<ProjectDto>> result = await projectService.SearchAsync(query, page, pageSize, null, null, null, types, order, User.GetName(), true);
        if (result.Data is null) return NotFound();
        if (result.Success is false) return HandleFailedResult(result);
        
        return HandleResult(mapper.Map<PagedResult<List<ProjectResponse>>>(result));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(string id, [FromQuery] bool categories = false, [FromQuery] bool users = false) 
    {
        IResult<ProjectDto?> result = await projectService.GetByIdOrSlugAsync(id, categories, users);
        return result is { Success: false, Data: null } ? NotFound() : HandleResult(result);
    }
    
    [Authorize(Policy = "administrator")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        IResult<bool> result = await projectService.DeleteAsync(id);
        return result.Success ? NoContent() : NotFound();
    }
    
    [Authorize(Policy = "creator")]
    [HttpPatch("{slug}")]
    public async Task<IActionResult> UpdateProject(string slug, [FromBody] UpdateProjectRequest request)
    {
        // Get the project by slug
        ServiceResult<ProjectDto?> userProjectResult = await projectService.GetBySlugAsync(slug, true);
        
        // If the project doesn't exist, return a 404
        if (userProjectResult.Success is false || userProjectResult.Data is null) return NotFound();
        
        // If the user isn't in the project, return a 403
        if (userProjectResult.Data.Users.Any(u => u.Name.EqualsIgnoreCase(User.GetName())) is false) return Forbid();
        
        // Update the project
        IResult<ProjectDto?> result = await projectService.UpdateAsync(slug, mapper.Map<ProjectDto>(request));
        
        // Return the result
        return result.Success ? Ok(mapper.Map<ProjectResponse>(result.Data)) : HandleFailedResult(result);
    }
}