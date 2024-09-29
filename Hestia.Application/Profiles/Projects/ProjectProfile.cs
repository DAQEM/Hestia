using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Projects;

namespace Hestia.Application.Profiles.Projects;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Loaders, opt => opt.MapFrom(src => src.Loaders.ToString().Split(new[] { ", " }, StringSplitOptions.None)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString().ToLower()))
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories.Select(c => c.Name).ToArray()))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users.ToList()));
        
        CreateMap<ProjectDto, Project>()
            .ForMember(dest => dest.Loaders, opt => opt.MapFrom(src => TryParseLoaders(src.Loaders)))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => TryParseProjectType(src.Type)));
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