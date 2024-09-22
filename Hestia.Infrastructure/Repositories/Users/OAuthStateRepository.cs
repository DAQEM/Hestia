using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Users;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories.Users;

public class OAuthStateRepository(HestiaDbContext dbContext) : IOAuthStateRepository
{
    public async Task<OAuthState?> GetAsync(int id)
    {
        return await dbContext.OAuthStates.FindAsync(id);
    }

    public Task<OAuthState?> GetByStateAsync(string state)
    {
        return dbContext.OAuthStates.FirstOrDefaultAsync(s => s.State == state);
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

    public Task<OAuthState> UpdateAsync(int id, OAuthState entity)
    {
        dbContext.OAuthStates.Update(entity);
        return Task.FromResult(entity);
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