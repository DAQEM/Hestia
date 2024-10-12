using AutoMapper;
using Hestia.Application.Dtos.Auth;
using Hestia.Application.Models.Responses.Auth.Sessions;
using Hestia.Domain.Models.Auth;

namespace Hestia.Application.Profiles.Auth;

public class SessionProfile : Profile
{
    public SessionProfile()
    {
        CreateMap<Session, SessionDto>();
        CreateMap<SessionDto, Session>();

        CreateMap<SessionDto, SessionResponse>();
    }
}