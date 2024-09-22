using AutoMapper;
using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Users;

namespace Hestia.Application.Profiles.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}