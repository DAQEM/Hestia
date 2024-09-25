using AutoMapper;
using Hestia.Application.Dtos.Blogs;
using Hestia.Domain.Models.Blogs;

namespace Hestia.Application.Profiles.Blogs;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        CreateMap<Blog, BlogDto>();
        CreateMap<BlogDto, Blog>();
    }
}