using System.ComponentModel.DataAnnotations;

namespace Hestia.Domain.Models;

public abstract class Model<TKey> where TKey : IEquatable<TKey>
{
    [Required, Key]
    public TKey Id { get; set; } = default!;
}