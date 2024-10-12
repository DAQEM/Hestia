using System.ComponentModel;
using System.Reflection;
using Hestia.Domain.Models.Auth;
using Hestia.Domain.Models.Blogs;
using Hestia.Domain.Models.Projects;
using Hestia.Domain.Models.Servers;
using Hestia.Domain.Models.Users;
using Hestia.Domain.Models.Wikis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hestia.Infrastructure.Database;

public class HestiaDbContext : DbContext
{
    public HestiaDbContext(DbContextOptions<HestiaDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Session> Sessions { get; set; } = null!;
    public DbSet<OAuthState> OAuthStates { get; set; } = null!;

    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<ProjectCategory> ProjectCategories { get; set; } = null!;
    public DbSet<ProjectVersion> ProjectVersions { get; set; } = null!;
    
    public DbSet<Blog> Blogs { get; set; } = null!;
    public DbSet<BlogCategory> BlogCategories { get; set; } = null!;
    public DbSet<BlogComment> BlogComments { get; set; } = null!;
    
    public DbSet<Server> Servers { get; set; } = null!;
    
    public DbSet<Wiki> Wikis { get; set; } = null!;

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

        #region Auth Configuration

        builder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Session>()
            .HasIndex(s => s.Token)
            .IsUnique();
        
        builder.Entity<OAuthState>()
            .HasIndex(o => o.State)
            .IsUnique();
        
        #endregion
        #region User Configuration

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
            .HasMany(u => u.Blogs)
            .WithMany(p => p.Users)
            .UsingEntity(j => j.ToTable("UserBlog"));

        builder.Entity<User>()
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Project>()
            .HasMany(p => p.Blogs)
            .WithMany(p => p.Projects)
            .UsingEntity(j => j.ToTable("BlogProject"));

        builder.Entity<Blog>()
            .HasMany(p => p.Comments)
            .WithOne(c => c.Blog)
            .HasForeignKey(c => c.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Blog>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Blogs)
            .UsingEntity(j => j.ToTable("BlogCategory"));

        builder.Entity<BlogComment>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BlogCategory>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Blog>()
            .HasIndex(p => p.Slug)
            .IsUnique();

        builder.Entity<BlogCategory>()
            .HasIndex(c => c.Slug)
            .IsUnique();
        
        #endregion
        #region Server Configuration
        
        builder.Entity<Server>()
            .HasIndex(s => s.Slug)
            .IsUnique();
        
        builder.Entity<Server>()
            .HasMany(s => s.Projects)
            .WithMany(p => p.Servers)
            .UsingEntity(j => j.ToTable("ServerProject"));
        
        builder.Entity<Server>()
            .HasMany(s => s.Users)
            .WithMany(u => u.Servers)
            .UsingEntity(j => j.ToTable("UserServer"));
        
        builder.Entity<Server>()
            .HasIndex(s => s.Host)
            .IsUnique();
        
        #endregion
        #region Wiki Configuration
        
        builder.Entity<Wiki>()
            .HasOne(w => w.Parent)
            .WithMany(w => w.Children)
            .HasForeignKey(w => w.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Wiki>()
            .HasOne(w => w.Project)
            .WithMany(p => p.Wikis)
            .HasForeignKey(w => w.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Entity<Wiki>()
            .HasMany(w => w.Authors)
            .WithMany(u => u.Wikis)
            .UsingEntity(j => j.ToTable("WikiAuthor"));
        
        builder.Entity<Wiki>()
            .HasIndex(w => w.Slug)
            .IsUnique();

        builder.Entity<Wiki>()
            .HasIndex(w => w.ProjectId);
        
        builder.Entity<Wiki>()
            .HasIndex(w => w.ParentId);
        
        #endregion
    }
}