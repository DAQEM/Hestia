using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Result;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Repositories.Blogs;
using Hestia.Domain.Result;

namespace Hestia.Application.Services.Blogs;

public class BlogCategoryService(IBlogCategoryRepository blogCategoryRepository, IMapper mapper)
{
    public async Task<IResult<List<BlogCategoryDto>>> GetAllAsync()
    {
        IEnumerable<BlogCategory> blogCategories = (await blogCategoryRepository.GetAllAsync()).Where(blogCategory => blogCategory.ParentId is null);
        
        return new ServiceResult<List<BlogCategoryDto>>
        {
            Data = blogCategories.Select(mapper.Map<BlogCategoryDto>).ToList(),
            Success = true,
            Message = "Blog categories found"
        };
    }

    public async Task<IResult<BlogCategoryDto?>> GetAsync(int id)
    {
        BlogCategory? blogCategory = await blogCategoryRepository.GetAsync(id);

        if (blogCategory is null)
        {
            return new ServiceResult<BlogCategoryDto?>
            {
                Success = false,
                Message = "Blog category not found"
            };
        }

        return new ServiceResult<BlogCategoryDto?>
        {
            Data = mapper.Map<BlogCategoryDto>(blogCategory),
            Success = true,
            Message = "Blog category found"
        };
    }

    public async Task<IResult<BlogCategoryDto>> AddAsync(BlogCategoryDto projectCategory)
    {
        BlogCategory blogCategory = mapper.Map<BlogCategory>(projectCategory);
        BlogCategory addedBlogCategory = await blogCategoryRepository.AddAsync(blogCategory);
        await blogCategoryRepository.SaveChangesAsync();

        return new ServiceResult<BlogCategoryDto>
        {
            Data = mapper.Map<BlogCategoryDto>(addedBlogCategory),
            Success = true,
            Message = "Blog category added"
        };
    }
}