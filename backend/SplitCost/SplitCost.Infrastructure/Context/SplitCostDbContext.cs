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
    public DbSet<ExpenseShare> ExpenseShares { get; set; }

    public SplitCostDbContext(DbContextOptions<SplitCostDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ResidenceMember>()
            .HasKey(rm => new { rm.UserId, rm.ResidenceId });

        modelBuilder.Entity<ResidenceMember>()
            .HasOne(rm => rm.User)
            .WithMany(u => u.Residences)
            .HasForeignKey(rm => rm.UserId);

        modelBuilder.Entity<ResidenceMember>()
            .HasOne(rm => rm.Residence)
            .WithMany(r => r.Members)
            .HasForeignKey(rm => rm.ResidenceId);

        // Expense -> Residence
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Residence)
            .WithMany(r => r.Expenses)
            .HasForeignKey(e => e.ResidenceId);

        // Expense -> User (quem pagou)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.PaidByUser)
            .WithMany(u => u.ExpensesPaid)
            .HasForeignKey(e => e.PaidByUserId)
            .OnDelete(DeleteBehavior.Restrict); // evita cascade delete indesejado

        // ExpenseShare -> Expense
        modelBuilder.Entity<ExpenseShare>()
            .HasOne(es => es.Expense)
            .WithMany(e => e.Shares)
            .HasForeignKey(es => es.ExpenseId);

        // ExpenseShare -> User
        modelBuilder.Entity<ExpenseShare>()
            .HasOne(es => es.User)
            .WithMany(u => u.ExpenseShares)
            .HasForeignKey(es => es.UserId);

        // Enum mapeado como string
        modelBuilder.Entity<Expense>()
            .Property(e => e.Type)
            .HasConversion<string>();
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
