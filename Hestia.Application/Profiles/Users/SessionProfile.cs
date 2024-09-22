using AutoMapper;
using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Users;

namespace Hestia.Application.Profiles.Users;

public class SessionProfile : Profile
{
    public SessionProfile()
    {
        CreateMap<Session, SessionDto>();
        CreateMap<SessionDto, Session>();
    }
}