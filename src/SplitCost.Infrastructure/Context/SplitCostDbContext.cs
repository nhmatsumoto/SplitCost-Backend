using Microsoft.EntityFrameworkCore;
using SplitCost.Domain.Common;
using SplitCost.Domain.Entities;

namespace SpitCost.Infrastructure.Context;

public class SplitCostDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<Residence> Residences { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }

    public SplitCostDbContext(DbContextOptions<SplitCostDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ================================
        // User
        // ================================
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.HasMany(u => u.Members)
                  .WithOne(m => m.User)
                  .HasForeignKey(m => m.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(u => u.Incomes)
                  .WithOne(i => i.User)
                  .HasForeignKey(i => i.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

        });

        // ================================
        // Residence
        // ================================
        modelBuilder.Entity<Residence>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasMany(r => r.Members)
                  .WithOne(m => m.Residence)
                  .HasForeignKey(m => m.ResidenceId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(r => r.Incomes)
                  .WithOne(i => i.Residence)
                  .HasForeignKey(i => i.ResidenceId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(r => r.Expenses)
                  .WithOne(e => e.Residence)
                  .HasForeignKey(e => e.ResidenceId)
                  .OnDelete(DeleteBehavior.Restrict);

        });

        // ================================
        // Income
        // ================================
        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.HasOne(i => i.User)
                  .WithMany(u => u.Incomes)
                  .HasForeignKey(i => i.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Residence)
                  .WithMany(r => r.Incomes)
                  .HasForeignKey(i => i.ResidenceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ================================
        // Expense
        // ================================
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Residence)
                  .WithMany(r => r.Expenses)
                  .HasForeignKey(e => e.ResidenceId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // ================================
        // Member
        // ================================
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(m => new { m.UserId, m.ResidenceId });

            entity.HasOne(m => m.User)
                  .WithMany(u => u.Members)
                  .HasForeignKey(m => m.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Residence)
                  .WithMany(r => r.Members)
                  .HasForeignKey(m => m.ResidenceId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ================================
        // UserSettings
        // ================================
        modelBuilder.Entity<UserSettings>(entity =>
        {
            entity.HasKey(us => us.Id);

            entity.HasOne(us => us.User)
                  .WithOne()
                  .HasForeignKey<UserSettings>(us => us.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.Property(us => us.Theme).HasMaxLength(50);
            entity.Property(us => us.Language).HasMaxLength(10);
        });
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateTimestamps();
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateTimestamps();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
