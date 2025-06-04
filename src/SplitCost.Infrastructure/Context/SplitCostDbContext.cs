using Microsoft.EntityFrameworkCore;
using SplitCost.Domain.Common;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SpitCost.Infrastructure.Context;

public class SplitCostDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ResidenceEntity> Residences { get; set; }
    public DbSet<MemberEntity> Members { get; set; }
    public DbSet<ExpenseEntity> Expenses { get; set; }

    public SplitCostDbContext(DbContextOptions<SplitCostDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USER ⇄ RESIDENCEMEMBER (Many-to-Many via Entity)
        modelBuilder.Entity<MemberEntity>()
            .HasKey(m => m.Id);

        // Cada usuário pode ter apenas 1 residência
        modelBuilder.Entity<MemberEntity>()
            .HasOne(m => m.User)
            .WithOne(u => u.Member)
            .HasForeignKey<MemberEntity>(m => m.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MemberEntity>()
            .HasOne(m => m.Residence)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.ResidenceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        // RESIDENCE ⇄ RESIDENCEEXPENSE (One-to-Many)
        modelBuilder.Entity<ExpenseEntity>()
            .HasOne(e => e.Residence)
            .WithMany(r => r.Expenses)
            .HasForeignKey(e => e.ResidenceId);

        // USER ⇄ RESIDENCEEXPENSE (RegisteredBy)
        modelBuilder.Entity<ExpenseEntity>()
            .HasOne(e => e.RegisteredBy)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.RegisteredByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // USER ⇄ RESIDENCEEXPENSE (PaidBy)
        modelBuilder.Entity<ExpenseEntity>()
            .HasOne(e => e.PaidBy)
            .WithMany(u => u.ResidenceExpensesPaid)
            .HasForeignKey(e => e.PaidByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // RESIDENCE
        modelBuilder.Entity<ResidenceEntity>()
            .HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedByUserId);

    }


    /// <summary>
    /// Salva as alterações no contexto e atualiza os campos de auditoria (timestamps)
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Salva as alterações no contexto de forma assíncrona e atualiza os campos de auditoria (timestamps)
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
