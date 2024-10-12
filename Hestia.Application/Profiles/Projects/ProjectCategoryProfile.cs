using AutoMapper;
using Hestia.Application.Dtos.Projects;
using Hestia.Application.Models.Requests;
using Hestia.Application.Models.Requests.Projects;
using Hestia.Domain.Models.Projects;

namespace Hestia.Application.Profiles.Projects;

public class ProjectCategoryProfile : Profile
{
    public ProjectCategoryProfile()
    {
        CreateMap<ProjectCategory, ProjectCategoryDto>();
        CreateMap<ProjectCategoryDto, ProjectCategory>();
        
        CreateMap<CreateProjectCategoryRequest, ProjectCategoryDto>();
    }
}