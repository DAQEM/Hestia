using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Models.Requests.Blogs;
using Hestia.Domain.Models.Blogs;

namespace Hestia.Application.Profiles.Blogs;

public class BlogCategoryProfile : Profile
{
    public BlogCategoryProfile()
    {
        CreateMap<BlogCategory, BlogCategoryDto>();
        CreateMap<BlogCategoryDto, BlogCategory>();

        CreateMap<CreateBlogCategoryRequest, BlogCategoryDto>();
    }
}