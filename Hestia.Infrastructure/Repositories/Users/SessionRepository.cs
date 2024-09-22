using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Users;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories.Users;

public class SessionRepository(HestiaDbContext dbContext) : ISessionRepository
{
    public async Task<Session?> GetAsync(int id)
    {
        return await dbContext.Sessions.FindAsync(id);
    }

    public async Task<Session?> GetByTokenAsync(string token)
    {
        return await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == token);
    }

    public Task<User?> GetUserByTokenAsync(string token)
    {
        return dbContext.Sessions
            .Include(s => s.User)
            .Where(s => s.Token == token)
            .Select(s => s.User)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Session>> GetAllAsync()
    {
        return await dbContext.Sessions.ToListAsync();
    }

    public async Task<Session> AddAsync(Session entity)
    {
        await dbContext.Sessions.AddAsync(entity);
        return entity;
    }

    public Task<Session> UpdateAsync(int id, Session entity)
    {
        dbContext.Sessions.Update(entity);
        return Task.FromResult(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Session? session = await dbContext.Sessions.FindAsync(id);
        if (session is null) return false;
        dbContext.Sessions.Remove(session);
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}