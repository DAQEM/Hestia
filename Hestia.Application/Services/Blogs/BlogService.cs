using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Result;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Blogs;

public class BlogService(IBlogRepository blogRepository, IMapper mapper)
{
    public async Task<IResult<List<BlogDto>>> SearchBlogsAsync(string? query, int page, int pageSize, bool? isFeatured, string[]? categories, string? user)
    {
        PagedResult<List<Blog>> pagedResult = await blogRepository.SearchAsync(query, page, pageSize, isFeatured, categories, user);

        return new PagedResult<List<BlogDto>>
        {
            Data = pagedResult.Data?.Select(mapper.Map<BlogDto>).ToList() ?? [],
            Success = true,
            Message = "Blogs found",
            Page = page,
            PageSize = pageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages
        };
    }

    public async Task<IResult<BlogDto?>> GetByIdOrSlugAsync(string idOrSlug, bool categories, bool users)
    {
        Blog? blog = await blogRepository.GetByIdOrSlugAsync(idOrSlug, categories, users);

        return blog is null ? BlogNotFoundResult : BlogFoundResult(blog);
    }
    
    private static ServiceResult<BlogDto?> BlogNotFoundResult => new()
    {
        Data = null,
        Success = false,
        Message = "Blog not found"
    };
    
    private ServiceResult<BlogDto?> BlogFoundResult(Blog blog) => new()
    {
        Data = mapper.Map<BlogDto>(blog),
        Success = true,
        Message = "Blog found"
    };
}