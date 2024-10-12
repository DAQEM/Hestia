using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Models.Requests.Blogs;
using Hestia.Application.Services.Blogs;
using Hestia.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hestia.API.Controllers.V1.Blogs;

[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Route("api/v1/blogs/categories")]
public class BlogCategoryController(ILogger<HestiaController> logger, BlogCategoryService blogCategoryService, IMapper mapper) : HestiaController(logger)
{
    [HttpGet]
    public async Task<IActionResult> GetAllProjectCategories()
    {
        IResult<List<BlogCategoryDto>> result = await blogCategoryService.GetAllAsync();
        return HandleResult(result);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectCategory(int id)
    {
        IResult<BlogCategoryDto?> result = await blogCategoryService.GetAsync(id);
        return HandleResult(result);
    }
    
    [HttpPost]
    [Authorize(Policy = "administrator")]
    public async Task<IActionResult> AddProjectCategory(CreateBlogCategoryRequest request)
    {
        BlogCategoryDto projectCategory = mapper.Map<BlogCategoryDto>(request);
        IResult<BlogCategoryDto> result = await blogCategoryService.AddAsync(projectCategory);
        return HandleResult(result);
    }
}