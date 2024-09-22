using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Users;

namespace Hestia.Application.Services.Auth;

public class OAuthStateService(IOAuthStateRepository stateRepository)
{
    public async Task<OAuthState?> GetStateAsync(string state)
    {
        return await stateRepository.GetByStateAsync(state);
    }

    public async Task<string> AddStateAsync(OAuthProvider provider, string returnUrl, int? userId = null)
    {
        string state = TokenService.GenerateToken(32);
        
        await stateRepository.AddAsync(new OAuthState
        {
            State = state,
            ReturnUri = returnUrl,
            Provider = provider,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        });
        
        await stateRepository.SaveChangesAsync();
        
        return state;
    }
}