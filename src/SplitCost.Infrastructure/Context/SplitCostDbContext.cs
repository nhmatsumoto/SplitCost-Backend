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

        // USER ⇄ RESIDENCEMEMBER (Many-to-Many via Entity)
        modelBuilder.Entity<Member>()
            .HasKey(m => m.Id);

        // Cada usuário pode ter apenas 1 residência
        //modelBuilder.Entity<Member>()
        //    .HasOne(m => m.User)
        //    .WithOne(u => u.Residence)
        //    .HasForeignKey<Member>(m => m.UserId)
        //    .IsRequired()
        //    .OnDelete(DeleteBehavior.Restrict); // Evita cascata

        modelBuilder.Entity<Member>()
            .HasOne(m => m.Residence)
            .WithMany(r => r.Members)
            .HasForeignKey(m => m.ResidenceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Evita cascata

        // RESIDENCE ⇄ RESIDENCEEXPENSE (One-to-Many)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.Residence)
            .WithMany(r => r.Expenses)
            .HasForeignKey(e => e.ResidenceId)
            .OnDelete(DeleteBehavior.Cascade); // Permite cascata, já que despesas dependem da residência

        // INCOME
        modelBuilder.Entity<Income>()
            .HasKey(m => m.Id);

        modelBuilder.Entity<Income>()
            .HasOne(i => i.User)
            .WithMany(u => u.Incomes) // Relacionamento com a coleção de Incomes no User
            .HasForeignKey(i => i.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Evita cascata para evitar o erro

        modelBuilder.Entity<Income>()
            .HasOne(i => i.Residence)
            .WithMany(u => u.Incomes) // Relacionamento com a coleção de Incomes no User
            .HasForeignKey(i => i.ResidenceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Evita cascata para evitar o erro


        // USER ⇄ RESIDENCEEXPENSE (RegisteredBy)
        modelBuilder.Entity<Expense>()
            .HasOne(e => e.RegisteredBy)
            .WithMany(u => u.Expenses)
            .HasForeignKey(e => e.RegisteredByUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // Evita cascata

        //// USER ⇄ RESIDENCEEXPENSE (PaidBy)
        //modelBuilder.Entity<Expense>()
        //    .HasOne(e => e.PaidBy)
        //    .WithMany(u => u.ResidenceExpensesPaid)
        //    .HasForeignKey(e => e.PaidByUserId)
        //    .IsRequired()
        //    .OnDelete(DeleteBehavior.Restrict); // Evita cascata

       

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
