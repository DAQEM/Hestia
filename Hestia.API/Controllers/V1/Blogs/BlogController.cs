using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Services.Blogs;
using Hestia.Domain.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hestia.API.Controllers.V1.Blogs;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/blogs")]
public class BlogController(BlogService blogService, ILogger<HestiaController> logger) : HestiaController(logger)
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(
        Summary = "Get all blogs",
        Description = "Get all blogs",
        OperationId = "GetBlogs",
        Tags = ["Blogs"]
    )]
    public async Task<IActionResult> SearchBlogs([FromQuery] string? query, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? isFeatured = null, [FromQuery] string[]? categories = null, [FromQuery] string? user = null)
    {
        IResult<List<BlogDto>> result = await blogService.SearchBlogsAsync(query, page, pageSize, isFeatured, categories, user);
        return HandleResult(result);
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