using AutoMapper;
using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Users;

namespace Hestia.Application.Services.Auth;

public class SessionService(IMapper mapper, ISessionRepository sessionRepository)
{
    public async Task<SessionDto?> GetByTokenAsync(string token)
    {
        Session? session = await sessionRepository.GetByTokenAsync(token);
        return session is null ? null : mapper.Map<SessionDto>(session);
    }

    public async Task<UserDto?> GetUserByTokenAsync(string token)
    {
        User? user = await sessionRepository.GetUserByTokenAsync(token);
        return user is null ? null : mapper.Map<UserDto>(user);
    }

    public async Task AddAsync(SessionDto session)
    {
        await sessionRepository.AddAsync(mapper.Map<Session>(session));
        await sessionRepository.SaveChangesAsync();
    }
}