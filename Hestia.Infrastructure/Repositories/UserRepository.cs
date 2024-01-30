using Hestia.Domain.Models;
using Hestia.Domain.Repositories;
using Hestia.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Repositories;

public class UserRepository(HestiaDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetAsync(int id)
    {
        return await dbContext.Users.FindAsync(id);
    }
    
    public async Task<User?> GetUserWithAccountByEmailAsync(string email)
    {
        return await dbContext.Users
            .Include(u => u.Accounts)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task<User?> GetUserByUserNameAsync(string name)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Name == name);
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