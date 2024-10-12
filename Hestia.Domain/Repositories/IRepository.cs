using Hestia.Domain.Models;

namespace Hestia.Domain.Repositories;

public interface IRepository<T, in TKey> where T : Model<TKey> where TKey : IEquatable<TKey>
{
    Task<T?> GetAsync(TKey id);
    Task<List<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<bool> DeleteAsync(TKey id);
    Task SaveChangesAsync();
}