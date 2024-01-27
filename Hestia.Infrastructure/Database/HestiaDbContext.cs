using Hestia.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Database;

public class HestiaDbContext : DbContext
{
    public HestiaDbContext(DbContextOptions<HestiaDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; } = null!;
}