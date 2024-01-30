using Hestia.Domain.Models;

namespace Hestia.Domain.Repositories;

public interface IUserRepository : IRepository<User, int>
{
    Task<User?> GetUserWithAccountByEmailAsync(string email);
    
    Task<User?> GetUserByUserNameAsync(string name);
}