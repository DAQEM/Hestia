using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Repositories.Auth;

public interface ISessionRepository : IRepository<Session, int>
{
    Task<List<Session>> GetAllByUserAsync(int userId);
    Task<Session?> GetByTokenAsync(string token, bool ignoreExpiration = false);
    Task<User?> GetUserByTokenAsync(string token, bool ignoreExpiration = false);
    Task<bool> DeleteByTokenAsync(string token);
    Task UpdateAsync(int sessionId, Session session);
}