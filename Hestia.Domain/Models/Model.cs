namespace Hestia.Domain.Models;

public abstract class Model<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
}