using AutoMapper;
using Hestia.Application.Dtos.Users;
using Hestia.Application.Models.Responses.Users;
using Hestia.Domain.Models.Users;

namespace Hestia.Application.Profiles.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        
        CreateMap<UserDto, UserResponse>();
        CreateMap<UserDto, OwnUserResponse>();
    }
}