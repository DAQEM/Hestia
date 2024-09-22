using Hestia.Domain.Models.Users;

namespace Hestia.Domain.Repositories.Users;

public interface IOAuthStateRepository : IRepository<OAuthState, int>
{
    Task<OAuthState?> GetByStateAsync(string state);
}