using Microsoft.EntityFrameworkCore;
using SplitCost.Domain.Common;
using SplitCost.Domain.Entities;

namespace SpitCost.Infrastructure.Context;

public class SplitCostDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Residence> Residences { get; set; }
    public DbSet<ResidenceMember> ResidenceMembers { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ResidenceExpenseShare> ExpenseShares { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public SplitCostDbContext(DbContextOptions<SplitCostDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USER ⇄ RESIDENCEMEMBER (Many-to-Many via Entity)
        modelBuilder.Entity<ResidenceMember>()
            .HasKey(rm => rm.Id);

        modelBuilder.Entity<ResidenceMember>()
            .HasOne(rm => rm.User)
            .WithMany(u => u.Residences)
            .HasForeignKey(rm => rm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ResidenceMember>()
            .HasOne(rm => rm.Residence)
            .WithMany(r => r.Members)
            .HasForeignKey(rm => rm.ResidenceId)
            .OnDelete(DeleteBehavior.Cascade);

        // RESIDENCE ⇄ RESIDENCEEXPENSE (One-to-Many)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Residence)
            .WithMany(r => r.Expenses)
            .HasForeignKey(e => e.ResidenceId)
            .OnDelete(DeleteBehavior.Cascade);

        // USER ⇄ RESIDENCEEXPENSE (RegisteredBy)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.RegisteredBy)
            .WithMany(u => u.ResidenceExpensesRegistered)
            .HasForeignKey(e => e.RegisteredByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // USER ⇄ RESIDENCEEXPENSE (PaidBy)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.PaidBy)
            .WithMany(u => u.ResidenceExpensesPaid)
            .HasForeignKey(e => e.PaidByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // RESIDENCEEXPENSE ⇄ RESIDENCEEXPENSESHARE (One-to-Many)
        modelBuilder.Entity<ResidenceExpenseShare>()
            .HasOne(s => s.ResidenceExpense)
            .WithMany(e => e.Shares)
            .HasForeignKey(s => s.ResidenceExpenseId)
            .OnDelete(DeleteBehavior.Cascade);

        // USER ⇄ RESIDENCEEXPENSESHARE (One-to-Many)
        modelBuilder.Entity<ResidenceExpenseShare>()
            .HasOne(s => s.User)
            .WithMany(u => u.ExpenseShares)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // RESIDENCE
        modelBuilder.Entity<Residence>()
            .HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // ADDRRESS ⇄ RESIDENCE (One-to-One)
        modelBuilder.Entity<Residence>()
        .HasOne(r => r.Address)
        .WithOne(a => a.Residence)
        .HasForeignKey<Residence>(r => r.AddressId);

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
