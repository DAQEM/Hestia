using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Models.Requests.Projects;
using Hestia.Application.Models.Responses.Projects;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Result;

namespace Hestia.Application.Profiles.Projects;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Loaders,
                opt => opt.MapFrom(src => src.Loaders.ToString().Split(new[] { ", " }, StringSplitOptions.None)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString().ToLower()))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Slug).ToArray()))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.ToList()));

        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.Loaders, opt => opt.MapFrom(src => TryParseLoaders(src.Loaders)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => TryParseProjectType(src.Type)))
            .ForMember(dest => dest.Categories, opt => opt.Ignore())
            .ForMember(dest => dest.Users, opt => opt.Ignore());

        CreateMap<ProjectDto, ProjectResponse>();
        CreateMap<ProjectDto, SimpleProjectResponse>();
        
        CreateMap<UpdateProjectRequest, ProjectDto>();

        CreateMap<PagedResult<List<ProjectDto>>, PagedResult<List<SimpleProjectResponse>>>();
        CreateMap<PagedResult<List<ProjectDto>>, PagedResult<List<ProjectResponse>>>();
    }

    private static ProjectType TryParseProjectType(string type)
    {
        return Enum.TryParse(type, true, out ProjectType projectType) ? projectType : ProjectType.Unknown;
    }
    
    private static ProjectLoaders TryParseLoaders(IEnumerable<string> loaders)
    {
        ProjectLoaders flags = ProjectLoaders.None;

        foreach (string inputString in loaders)
        {
            try
            {
                ProjectLoaders parsedFlag = (ProjectLoaders)Enum.Parse(typeof(ProjectLoaders), inputString, true);
                flags |= parsedFlag;
            }
            catch (ArgumentException _) {}
        }

        return flags;
    }
}