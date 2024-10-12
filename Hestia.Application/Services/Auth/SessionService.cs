using AutoMapper;
using Hestia.Application.Dtos.Auth;
using Hestia.Application.Dtos.Users;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Auth;
using Hestia.Domain.Repositories.Users;

namespace Hestia.Application.Services.Auth;

public class SessionService(IMapper mapper, ISessionRepository sessionRepository, IUserRepository userRepository)
{
    public async Task<List<SessionDto>> GetAllByUserAsync(int userId)
    {
        List<Session> sessions = await sessionRepository.GetAllByUserAsync(userId);
        return mapper.Map<List<SessionDto>>(sessions);
    }

    public async Task<SessionDto?> GetByIdAsync(int userId, int id)
    {
        Session? session = await sessionRepository.GetAsync(id);
        return session?.UserId == userId ? mapper.Map<SessionDto>(session) : null;
    }

    public async Task<SessionDto?> GetByTokenAsync(string token, bool ignoreExpiration = false)
    {
        Session? session = await sessionRepository.GetByTokenAsync(token, ignoreExpiration);
        return session is null ? null : mapper.Map<SessionDto>(session);
    }

    public async Task<UserDto?> GetUserByTokenAsync(string token, bool ignoreExpiration = false)
    {
        User? user = await sessionRepository.GetUserByTokenAsync(token, ignoreExpiration);

        if (user is null) return null;
        
        await userRepository.UpdateLastActiveAsync(user.Id, DateTime.UtcNow);
        await userRepository.SaveChangesAsync();
        
        return mapper.Map<UserDto>(user);
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

    public async Task<bool> DeleteByIdAsync(int id)
    {
        bool result = await sessionRepository.DeleteAsync(id);
        await sessionRepository.SaveChangesAsync();
        return result;
    }
}