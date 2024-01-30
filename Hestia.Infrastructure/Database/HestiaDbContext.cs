using Hestia.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Hestia.Infrastructure.Database;

public class HestiaDbContext : DbContext
{
    public HestiaDbContext(DbContextOptions<HestiaDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Product> Products { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(HestiaDbContext).Assembly);
        
        builder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Account>()
            .HasIndex(a => new {a.Provider, a.ProviderAccountId})
            .IsUnique();
    }
}