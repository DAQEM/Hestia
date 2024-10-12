using Hestia.Domain.Models.Auth;
using Hestia.Domain.Repositories.Auth;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories.Auth;

public class OAuthStateRepository(HestiaDbContext dbContext) : IOAuthStateRepository
{
    public async Task<OAuthState?> GetAsync(int id)
    {
        return await dbContext.OAuthStates.FindAsync(id);
    }

    public async Task<OAuthState?> GetByStateAsync(string state)
    {
        OAuthState? result = await dbContext.OAuthStates
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.State == state);
        
        if (result is not null)
        {
            dbContext.OAuthStates.Remove(result);
            
            await dbContext.OAuthStates
                .Where(s => s.ExpiresAt < DateTime.UtcNow)
                .ForEachAsync(s => dbContext.OAuthStates.Remove(s));
            
            await dbContext.SaveChangesAsync();
        }
        
        return result;
    }

    public async Task<List<OAuthState>> GetAllAsync()
    {
        return await dbContext.OAuthStates.ToListAsync();
    }

    public async Task<OAuthState> AddAsync(OAuthState entity)
    {
        await dbContext.OAuthStates.AddAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        OAuthState? state = await dbContext.OAuthStates.FindAsync(id);
        if (state is null) return false;
        dbContext.OAuthStates.Remove(state);
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}