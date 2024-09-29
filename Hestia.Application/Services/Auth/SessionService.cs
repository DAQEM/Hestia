using AutoMapper;
using Hestia.Application.Dtos.Auth;
using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Auth;

namespace Hestia.Application.Services.Auth;

public class SessionService(IMapper mapper, ISessionRepository sessionRepository)
{
    public async Task<SessionDto?> GetByTokenAsync(string token, bool ignoreExpiration = false)
    {
        Session? session = await sessionRepository.GetByTokenAsync(token, ignoreExpiration);
        return session is null ? null : mapper.Map<SessionDto>(session);
    }

    public async Task<UserDto?> GetUserByTokenAsync(string token, bool ignoreExpiration = false)
    {
        User? user = await sessionRepository.GetUserByTokenAsync(token, ignoreExpiration);
        return user is null ? null : mapper.Map<UserDto>(user);
    }

    public async Task AddAsync(SessionDto session)
    {
        await sessionRepository.AddAsync(mapper.Map<Session>(session));
        await sessionRepository.SaveChangesAsync();
    }

    public async Task UpdateAsync(SessionDto session)
    {
        await sessionRepository.UpdateAsync(session.Id, mapper.Map<Session>(session));
        await sessionRepository.SaveChangesAsync();
    }

    public async Task<bool> DeleteByTokenAsync(string token)
    {
        bool result = await sessionRepository.DeleteByTokenAsync(token);
        await sessionRepository.SaveChangesAsync();
        return result;
    }
}