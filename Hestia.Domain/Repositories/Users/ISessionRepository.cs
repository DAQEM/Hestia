using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Repositories.Users;

public interface ISessionRepository : IRepository<Session, int>
{
    Task<Session?> GetByTokenAsync(string token);
    Task<User?> GetUserByTokenAsync(string token);
}