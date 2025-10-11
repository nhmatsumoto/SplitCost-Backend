using Microsoft.EntityFrameworkCore;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Domain.Common;
using SplitCost.Domain.Entities;
using System.Reflection;

namespace SplitCost.Infrastructure.Context;

public class SplitCostDbContext : DbContext
{
    // DbSets
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<Residence> Residences { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }

    // Services
    private readonly ITenantService _tenantService;

    public SplitCostDbContext(DbContextOptions<SplitCostDbContext> options, ITenantService tenantService)
        : base(options)
    {
        _tenantService = tenantService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
            // UserId é UUID do Keycloak, sem FK
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
            entity.HasKey(m => m.Id); // usamos Id como PK, UserId e ResidenceId apenas propriedades

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
            // UserId é UUID do Keycloak, sem relação de navegação
            entity.Property(us => us.Theme).HasMaxLength(50);
            entity.Property(us => us.Language).HasMaxLength(10);
        });

        modelBuilder.HasPostgresExtension("uuid-ossp");

        ApplyGlobalTenantFilter(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyTenantId();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                entry.Entity.UpdateTimestamps();
        }

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantId();

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                entry.Entity.UpdateTimestamps();
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyGlobalTenantFilter(ModelBuilder modelBuilder)
    {
        var method = typeof(SplitCostDbContext)
            .GetMethod(nameof(SetTenantFilter), BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseTenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                method?
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder });
            }
        }
    }

    private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseTenantEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.ResidenceId == _tenantService.GetCurrentTenantId());
    }

    private void ApplyTenantId()
    {
        var tenantId = _tenantService.GetCurrentTenantId();

        foreach (var entry in ChangeTracker.Entries<BaseTenantEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.ResidenceId = tenantId;
        }
    }
}
