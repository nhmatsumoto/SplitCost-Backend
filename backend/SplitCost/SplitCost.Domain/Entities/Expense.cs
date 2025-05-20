using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Expenses")]
public class Expense : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public ExpenseType Type { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsSharedAmongMembers { get; set; }
    public string Description { get; set; } = null!;

    // Navigation EF Core

    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; }

    [ForeignKey("RegisteredBy")]
    [Column("RegisteredByUserId")]
    public Guid RegisteredByUserId { get; set; }
    public User RegisteredBy { get; set; } = null!;

    [ForeignKey("PaidBy")]
    [Column("PaidByUserId")]
    public Guid PaidByUserId { get; set; }
    public User PaidBy { get; set; } = null!;

    public ICollection<ResidenceExpenseShare> Shares { get; set; } = new List<ResidenceExpenseShare>();


    public Expense()
    {
        
    }

    public Expense(
        ExpenseType type,
        ExpenseCategory category,
        decimal amount,
        DateTime date,
        bool isSharedAmongMembers,
        string description,
        Guid residenceId,
        Guid registeredByUserId,
        Guid paidByUserId)
    {
        if(string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição da despesa não pode ser vazia.");
        if (amount <= 0)
            throw new ArgumentException("O valor da despesa deve ser maior que zero.");
        if (residenceId == Guid.Empty)
            throw new ArgumentException("É necessário informar uma residência.");
        if (registeredByUserId == Guid.Empty)
            throw new ArgumentException("É necessário informar um usuário que registrou a despesa.");
        if (paidByUserId == Guid.Empty)
            throw new ArgumentException("É necessário informar um usuário que pagou a despesa.");
        Type = type;
        Category = category;
        Amount = amount;
        Date = date;
        IsSharedAmongMembers = isSharedAmongMembers;
        Description = description;
        ResidenceId = residenceId;
        RegisteredByUserId = registeredByUserId;
        PaidByUserId = paidByUserId;
    }
}
