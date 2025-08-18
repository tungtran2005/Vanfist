using Microsoft.EntityFrameworkCore;
using Vanfist.Entities;
using Vanfist.Models;

namespace Vanfist.Configuration;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<ActionEntity> Actions { get; set; }
    public DbSet<Resource> Resources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Account entity
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Number).HasMaxLength(20);

            // Unique constraints
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Configure Role entity
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);

            // Unique constraint
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Permission entity
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);

            // Foreign key relationships
            entity.HasOne(e => e.Action)
                .WithMany(a => a.Permissions)
                .HasForeignKey(e => e.ActionId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Resource)
                .WithMany(r => r.Permissions)
                .HasForeignKey(e => e.ResourceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint for permission name
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure ActionEntity
        modelBuilder.Entity<ActionEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);

            // Unique constraint
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure Resource entity
        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);

            // Unique constraint
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure many-to-many relationships

        // Account <-> Role (many-to-many)
        modelBuilder.Entity<Account>()
            .HasMany(a => a.Roles)
            .WithMany(r => r.Accounts)
            .UsingEntity(
                "AccountRoles",
                l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                r => r.HasOne(typeof(Account)).WithMany().HasForeignKey("AccountId")
                    .HasPrincipalKey(nameof(Account.Id)),
                j =>
                {
                    j.HasKey("AccountId", "RoleId");
                    j.ToTable("AccountRoles");
                });

        // Role <-> Permission (many-to-many)
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(
                "RolePermissions",
                l => l.HasOne(typeof(Permission)).WithMany().HasForeignKey("PermissionId")
                    .HasPrincipalKey(nameof(Permission.Id)),
                r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
                j =>
                {
                    j.HasKey("RoleId", "PermissionId");
                    j.ToTable("RolePermissions");
                });
    }
}