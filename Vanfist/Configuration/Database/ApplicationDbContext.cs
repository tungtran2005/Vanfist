using Microsoft.EntityFrameworkCore;
using Vanfist.Entities;

namespace Vanfist.Configuration.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Attachment> Attachments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Account - Role (many-to-many)
        modelBuilder.Entity<Account>()
            .HasMany(a => a.Roles)
            .WithMany(r => r.Accounts)
            .UsingEntity(j => j.ToTable("AccountRoles"));

        // Account - Address (one-to-many)
        modelBuilder.Entity<Address>()
            .HasOne(a => a.Account)
            .WithMany(acc => acc.Addresses)
            .HasForeignKey(a => a.AccountId);

        // Account - Invoice (one-to-many)
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Account)
            .WithMany(a => a.Invoices)
            .HasForeignKey(i => i.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Category - Car (one-to-many)
        modelBuilder.Entity<Model>()
            .HasOne(c => c.Category)
            .WithMany(cat => cat.Models)
            .HasForeignKey(c => c.CategoryId);

        // Car - Attachment (one-to-many)
        modelBuilder.Entity<Attachment>()
            .HasOne(a => a.Model)
            .WithMany(c => c.Attachments)
            .HasForeignKey(a => a.ModelId)
            .OnDelete(DeleteBehavior.Cascade);

        // Car - Invoice (one-to-many)
        modelBuilder.Entity<Invoice>()
            .HasOne(i => i.Model)
            .WithMany()
            .HasForeignKey(i => i.ModelId)
            .OnDelete(DeleteBehavior.Restrict);



        // Account (1) - (n) Order
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Account)
            .WithMany(a => a.Orders)
            .HasForeignKey(o => o.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        // Order (1) - (1) OrderItem
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Item)
            .WithOne(oi => oi.Order)
            .HasForeignKey<OrderItem>(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Model (1) - (n) OrderItem
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Model)
            .WithMany(m => m.OrderItems)
            .HasForeignKey(oi => oi.ModelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}