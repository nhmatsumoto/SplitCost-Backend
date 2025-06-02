using Microsoft.EntityFrameworkCore;
using SplitCost.Domain.Common;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Entities;

namespace SpitCost.Infrastructure.Context;

public class SplitCostDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ResidenceEntity> Residences { get; set; }
    public DbSet<MemberEntity> Members { get; set; }
    public DbSet<ExpenseEntity> Expenses { get; set; }
    public DbSet<ExpenseShareEntity> ExpenseShares { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }

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
            .IsRequired();

        modelBuilder.Entity<MemberEntity>()
            .HasOne(m => m.Residence)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.ResidenceId)
            .IsRequired();

        // RESIDENCE ⇄ RESIDENCEEXPENSE (One-to-Many)
        modelBuilder.Entity<ExpenseEntity>()
            .HasOne(e => e.Residence)
            .WithMany(r => r.Expenses)
            .HasForeignKey(e => e.ResidenceId);

        // USER ⇄ RESIDENCEEXPENSE (RegisteredBy)
        modelBuilder.Entity<ExpenseEntity>()
            .HasOne(e => e.RegisteredBy)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.RegisteredByUserId);

        // USER ⇄ RESIDENCEEXPENSE (PaidBy)
        modelBuilder.Entity<ExpenseEntity>()
            .HasOne(e => e.PaidBy)
            .WithMany(u => u.ResidenceExpensesPaid)
            .HasForeignKey(e => e.PaidByUserId);

        // RESIDENCEEXPENSE ⇄ RESIDENCEEXPENSESHARE (One-to-Many)
        modelBuilder.Entity<ExpenseShareEntity>()
            .HasOne(s => s.Expense)
            .WithMany(e => e.Shares)
            .HasForeignKey(s => s.ExpenseId);

        // USER ⇄ RESIDENCEEXPENSESHARE (One-to-Many)
        modelBuilder.Entity<ExpenseShareEntity>()
            .HasOne(s => s.User)
            .WithMany(u => u.ExpenseShares)
            .HasForeignKey(s => s.UserId);

        // RESIDENCE
        modelBuilder.Entity<ResidenceEntity>()
            .HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedByUserId);

        // ADDRRESS ⇄ RESIDENCE (One-to-One)
        modelBuilder.Entity<ResidenceEntity>()
        .HasOne(r => r.Address)
        .WithOne(a => a.Residence)
        .HasForeignKey<ResidenceEntity>(r => r.AddressId);

    }


    public override int SaveChanges()
    {
        return base.SaveChanges();
    }


    /// <summary>
    /// Atualiza campos de auditoria
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.UpdateTimestamps();
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdateTimestamps();
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

}
