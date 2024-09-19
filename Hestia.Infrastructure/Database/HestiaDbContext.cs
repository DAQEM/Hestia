using System.ComponentModel;
using System.Reflection;
using Hestia.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hestia.Infrastructure.Database;

public class HestiaDbContext : DbContext
{
    public HestiaDbContext(DbContextOptions<HestiaDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectCategory> ProjectCategories { get; set; } = null!;
    public DbSet<ProjectVersion> ProjectVersions { get; set; } = null!;
    
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<PostCategory> PostCategories { get; set; } = null!;
    public DbSet<PostComment> PostComments { get; set; } = null!;
    public DbSet<PostMeta> PostMeta { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
        {
            foreach (IMutableProperty property in entityType.GetProperties())
            {
                MemberInfo? memberInfo = property.PropertyInfo ?? (MemberInfo?) property.FieldInfo;
                if (memberInfo == null) continue;
                DefaultValueAttribute? defaultValue = Attribute.GetCustomAttribute(memberInfo, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
                if (defaultValue == null) continue;                   
                property.SetDefaultValue(defaultValue.Value);
            }
        }

        builder.ApplyConfigurationsFromAssembly(typeof(HestiaDbContext).Assembly);

        #region User Configuration
        
        builder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Account>()
            .HasIndex(a => new { a.Provider, a.ProviderAccountId })
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(u => u.Name)
            .IsUnique();

        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        #endregion
        #region Project Configuration

        builder.Entity<Project>()
            .HasIndex(p => p.Name)
            .IsUnique();

        builder.Entity<Project>()
            .HasIndex(p => p.CurseForgeId)
            .IsUnique();

        builder.Entity<Project>()
            .HasIndex(p => p.ModrinthId)
            .IsUnique();

        builder.Entity<Project>()
            .HasIndex(p => p.GitHubUrl)
            .IsUnique();

        builder.Entity<Project>()
            .HasIndex(p => p.CurseForgeUrl)
            .IsUnique();

        builder.Entity<Project>()
            .HasIndex(p => p.ModrinthUrl)
            .IsUnique();
        
        builder.Entity<Project>()
            .ToTable(t => t.HasCheckConstraint("CK_Project_CurseForgeId_ModrinthId", "((\"CurseForgeId\" IS NOT NULL) OR (\"ModrinthId\" IS NOT NULL))"));
        
        builder.Entity<Project>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Projects)
            .UsingEntity(j => j.ToTable("ProjectCategory"));
        
        builder.Entity<ProjectCategory>()
            .HasIndex(c => c.Slug)
            .IsUnique();
        
        builder.Entity<Project>()
            .HasMany(p => p.Versions)
            .WithMany(v => v.Projects)
            .UsingEntity(j => j.ToTable("ProjectVersion"));
        
        // builder.Entity<Project>()
        //     .HasMany(p => p.Users)
        //     .WithMany(u => u.Projects)
        //     .UsingEntity(j => j.ToTable("UserProject"));
        
        #endregion
        #region Blog Configuration
        
        builder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithMany(p => p.Users)
            .UsingEntity(j => j.ToTable("UserPost"));

        builder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Project>()
            .HasMany(p => p.Posts)
            .WithMany(p => p.Projects)
            .UsingEntity(j => j.ToTable("PostProject"));

        builder.Entity<Post>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasMany(p => p.Meta)
            .WithOne(m => m.Post)
            .HasForeignKey(m => m.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Posts)
            .UsingEntity(j => j.ToTable("PostCategory"));

        builder.Entity<PostComment>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PostCategory>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Post>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder.Entity<PostCategory>()
            .HasIndex(c => c.Slug)
            .IsUnique();
        
        #endregion
    }
}