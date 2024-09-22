using Hestia.Domain.Models.Users;
using Hestia.Domain.Repositories.Users;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories.Users;

public class UserRepository(HestiaDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetAsync(int id)
    {
        return await dbContext.Users.FindAsync(id);
    }
    
    public async Task<User?> GetUserByUserNameAsync(string name)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task<User?> GetByOAuthIdAsync(OAuthProvider provider, string userId)
    {
        return provider switch
        {
            OAuthProvider.Discord => long.TryParse(userId, out long discordId)
                ? await dbContext.Users.FirstOrDefaultAsync(u => u.DiscordId == discordId)
                : null,
            _ => throw new NotImplementedException()
        };
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User> AddAsync(User entity)
    {
        await dbContext.Users.AddAsync(entity);
        return entity;
    }

    public Task<User> UpdateAsync(int id, User entity)
    {
        dbContext.Users.Update(entity);
        return Task.FromResult(entity);
    }

    public async Task UpdateOAuthIdAsync(int userId, OAuthProvider provider, string oAuthUserId)
    {
        User? user = await GetAsync(userId);

        if (user is not null)
        {
            switch (provider)
            {
                case OAuthProvider.Discord:
                    bool isValid = long.TryParse(oAuthUserId, out long discordId);
                    if (isValid)
                    {
                        user.DiscordId = discordId;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            await UpdateAsync(userId, user);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        User? user = await dbContext.Users.FindAsync(id);
        if (user is null) return false;
        dbContext.Users.Remove(user);
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}