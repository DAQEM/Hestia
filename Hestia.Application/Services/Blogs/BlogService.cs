using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Result;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Blogs;

public class BlogService(IBlogRepository blogRepository, IMapper mapper)
{
    public async Task<IResult<List<BlogDto>>> SearchBlogsAsync(string? query, int page, int pageSize, string[]? categories, string? user, bool creator = false)
    {
        PagedResult<List<Blog>> pagedResult = await blogRepository.SearchAsync(query, page, pageSize, categories, user, creator);

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

    public async Task<IResult<BlogDto>> AddBlogAsync(BlogDto blogDto)
    {
        Blog blog = mapper.Map<Blog>(blogDto);
        Blog addedBlog = await blogRepository.AddAsync(blog);
        await blogRepository.SaveChangesAsync();
        return new ServiceResult<BlogDto>
        {
            Data = mapper.Map<BlogDto>(addedBlog),
            Success = true,
            Message = "Blog added"
        };
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