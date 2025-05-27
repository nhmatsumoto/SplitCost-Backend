using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Expenses")]
public class Expense : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    public ExpenseType Type { get; private set; }
    public ExpenseCategory Category { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }

    // A residência a qual a despesa pertence
    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; }

    // Quem registrou a despesa
    [ForeignKey("RegisteredBy")]
    [Column("RegisteredByUserId")]
    public Guid RegisteredByUserId { get; set; }
    public User RegisteredBy { get; set; } = null!;

    // Quem pagou a despesa
    [ForeignKey("PaidBy")]
    [Column("PaidByUserId")]
    public Guid PaidByUserId { get; set; }
    public User PaidBy { get; set; } = null!;

    public bool IsSharedAmongMembers { get; private set; }
    public ICollection<ExpenseShare> Shares { get; set; } = new List<ExpenseShare>();

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
        SetType(type);
        SetCategory(category);
        SetDescription(description);
        SetAmount(amount);
        SetResidenceId(residenceId);
        SetWhoPaidExpense(paidByUserId);
        SetWhoRegisterExpense(registeredByUserId);
    }

    public Expense SetType(ExpenseType type)
    {
        Type = type;
        return this;
    }

    public Expense SetCategory(ExpenseCategory category)
    {
        Category = category;
        return this;
    }

    public Expense SetAmount(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException("O valor da despesa deve ser maior que zero.");
        Amount = amount;
        return this;
    }

    public Expense SetDate(DateTime date)
    {
        Date = date;
        return this;
    }

    public Expense SetSharedAmongMembers(bool sharedAmongMembers)
    {
        IsSharedAmongMembers = sharedAmongMembers;
        return this;
    }

    public Expense SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("A descrição da despesa não pode ser vazia.");
        Description = description;
        return this;
    }

    public Expense SetResidenceId(Guid residenceId)
    {
        if (residenceId == Guid.Empty)
            throw new ArgumentException("É necessário informar uma residência.");
        ResidenceId = residenceId;
        return this;
    }

    public Expense SetWhoRegisterExpense(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("É necessário informar um usuário que registrou a despesa.");
        RegisteredByUserId = userId;
        return this;
    }

    public Expense SetWhoPaidExpense(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("É necessário informar um usuário que pagou a despesa.");
        PaidByUserId = userId;
        return this;
    }
}