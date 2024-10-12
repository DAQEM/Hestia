using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Auth;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories.Auth;

public class SessionRepository(HestiaDbContext dbContext) : ISessionRepository
{
    public async Task<Session?> GetAsync(int id)
    {
        return await dbContext.Sessions.FindAsync(id);
    }

    public Task<List<Session>> GetAllByUserAsync(int userId)
    {
        return dbContext.Sessions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .ToListAsync();
    }

    public async Task<Session?> GetByTokenAsync(string token, bool ignoreExpiration = false)
    {
        return await dbContext.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Token == token && (ignoreExpiration || s.ExpiresAt > DateTime.UtcNow));
    }

    public Task<User?> GetUserByTokenAsync(string token, bool ignoreExpiration = false)
    {
        return dbContext.Sessions
            .AsNoTracking()
            .Include(s => s.User)
            .Where(s => s.Token == token && (ignoreExpiration || s.ExpiresAt > DateTime.UtcNow))
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

    public async Task<bool> DeleteAsync(int id)
    {
        Session? session = await dbContext.Sessions.FindAsync(id);
        if (session is null) return false;
        dbContext.Sessions.Remove(session);
        return true;
    }

    public async Task<bool> DeleteByTokenAsync(string token)
    {
        Session? session = await dbContext.Sessions.FirstOrDefaultAsync(s => s.Token == token);
        if (session is null) return false;
        dbContext.Sessions.Remove(session);
        return true;
    }

    public async Task UpdateAsync(int sessionId, Session session)
    {
        Session? existing = await dbContext.Sessions.FindAsync(sessionId);

        if (existing is not null)
        {
            existing.Token = session.Token;
            existing.LastUsedAt = session.LastUsedAt;
            existing.ExpiresAt = session.ExpiresAt;
            existing.RefreshExpiresAt = session.RefreshExpiresAt;
        }
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}