using SplitCost.Domain.Common;
using SplitCost.Domain.Enums;
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
    public DateTime Date { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; private set; }

    [MaxLength(500)]
    [Required]
    public string Description { get; private set; } = null!;

    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; private set; }
    public Residence Residence { get; private set; } = null!;

    [ForeignKey("RegisteredBy")]
    [Column("RegisteredByUserId")]
    public Guid RegisteredByUserId { get; private set; }
    public User RegisteredBy { get; private set; } = null!;

    [ForeignKey("PaidBy")]
    public Guid PaidByUserId { get; private set; }
    public User PaidBy { get; private set; } = null!;
    internal Expense() { }

    internal Expense(
        ExpenseType type,
        ExpenseCategory category,
        decimal amount,
        DateTime date,
        string description,
        Guid residenceId,
        Guid registeredByUserId,
        Guid paidByUserId)
    {
        SetType(type);
        SetCategory(category);
        SetAmount(amount);
        SetDate(date);
        SetDescription(description);
        SetResidenceId(residenceId);
        SetWhoRegistered(registeredByUserId);
        SetWhoPaid(paidByUserId);
    }

    public Expense SetId(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id inválido.");
        Id = id;
        return this;
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
        if (amount <= 0) throw new ArgumentOutOfRangeException("O valor da despesa deve ser maior que zero.");
        Amount = amount;
        return this;
    }

    public Expense SetDate(DateTime date)
    {
        if (date == default) throw new ArgumentException("Data inválida.");
        Date = date;
        return this;
    }

    public Expense SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("A descrição não pode ser vazia.");
        Description = description.Trim();
        return this;
    }


    public Expense SetResidenceId(Guid residenceId)
    {
        if (residenceId == Guid.Empty) throw new ArgumentException("Residência inválida.");
        ResidenceId = residenceId;
        return this;
    }

    public Expense SetWhoRegistered(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Usuário inválido para registro.");
        RegisteredByUserId = userId;
        return this;
    }

    public Expense SetWhoPaid(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("Usuário inválido para pagamento.");
        PaidByUserId = userId;
        return this;
    }
}
