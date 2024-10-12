using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Models.Responses.Blogs;
using Hestia.Application.Services.Blogs;
using Hestia.Domain.Result;
using Hestia.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hestia.API.Controllers.V1.Blogs;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/blogs")]
public class BlogController(ILogger<HestiaController> logger, BlogService blogService, IMapper mapper) : HestiaController(logger)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Get all blogs",
        Description = "Get all blogs",
        OperationId = "GetBlogs",
        Tags = ["Blogs"]
    )]
    public async Task<IActionResult> SearchBlogs([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string[]? categories = null, [FromQuery] string? user = null)
    {
        IResult<List<BlogDto>> result = await blogService.SearchBlogsAsync(query, page, pageSize, categories, user);
        if (result.Data is null) return NotFound();
        if (result.Success is false) return HandleFailedResult(result);
         
        return HandleResult(mapper.Map<PagedResult<List<SimpleBlogResponse>>>(result));
    }
    
    [HttpGet("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Get all blogs by user",
        Description = "Get all blogs by user",
        OperationId = "GetUserBlogs",
        Tags = ["Blogs"]
    )]
    public async Task<IActionResult> SearchUserBlogs([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        IResult<List<BlogDto>> result = await blogService.SearchBlogsAsync(query, page, pageSize, null, User.GetName(), true);
        if (result.Data is null) return NotFound();
        if (result.Success is false) return HandleFailedResult(result);
        
        return HandleResult(mapper.Map<PagedResult<List<SimpleBlogResponse>>>(result));
    }
    
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get a blog by ID or slug",
        Description = "Get a blog by ID or slug",
        OperationId = "GetBlog",
        Tags = ["Blogs"]
    )]
    public async Task<IActionResult> GetBlog(string id, [FromQuery] bool categories = false, [FromQuery] bool users = false) 
    {
        IResult<BlogDto?> result = await blogService.GetByIdOrSlugAsync(id, categories, users);
        return HandleResult(result);
    }
}