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
    }
}