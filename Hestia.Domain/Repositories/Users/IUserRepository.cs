using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Repositories.Users;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetUserByUserNameAsync(string name);
    Task<User?> GetByOAuthIdAsync(OAuthProvider provider, string userId);
    Task UpdateOAuthIdAsync(int userId, OAuthProvider provider, string oAuthUserId);
}