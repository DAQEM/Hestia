using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Application.Models.Responses.Blogs;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Result;

namespace Hestia.Application.Profiles.Blogs;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>();
        CreateMap<BlogDto, Blog>();
        
        CreateMap<BlogDto, BlogResponse>();
        CreateMap<BlogDto, SimpleBlogResponse>();
        
        CreateMap<PagedResult<List<BlogDto>>, PagedResult<List<BlogResponse>>>();
        CreateMap<PagedResult<List<BlogDto>>, PagedResult<List<SimpleBlogResponse>>>();
    }
}