using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Repositories.Auth;

public interface ISessionRepository : IRepository<Session, int>
{
    Task<Session?> GetByTokenAsync(string token, bool ignoreExpiration = false);
    Task<User?> GetUserByTokenAsync(string token, bool ignoreExpiration = false);
    Task<bool> DeleteByTokenAsync(string token);
}