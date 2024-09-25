using Hestia.Domain.Models.Auth;

namespace Hestia.Domain.Repositories.Auth;

public interface IOAuthStateRepository : IRepository<OAuthState, int>
{
    Task<OAuthState?> GetByStateAsync(string state);
}