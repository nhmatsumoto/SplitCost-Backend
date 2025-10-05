using SplitCost.Domain.Common;
using SplitCost.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Expenses")]
public class Expense : BaseTenantEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [Required]
    [EnumDataType(typeof(ExpenseType))]
    public ExpenseType Type { get; private set; }

    [Required]
    [EnumDataType(typeof(ExpenseCategory))]
    public ExpenseCategory Category { get; private set; }

    [Required]
    public DateTime Date { get; private set; }

    [Required]
    [MaxLength(500)]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; private set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; private set; } = string.Empty;

    // UUID do usuário pagante (Keycloak)
    [Required]
    [Column("PayingUserId")]
    public Guid PayingUserId { get; private set; }

    // Relação com residência (tenant)
    [Required]
    public Residence Residence { get; private set; } = null!;

    public Expense() { }

    internal Expense(
        ExpenseType type,
        ExpenseCategory category,
        decimal amount,
        DateTime date,
        string description,
        Guid residenceId,
        Guid payingUserId)
    {
        SetType(type)
            .SetCategory(category)
            .SetAmount(amount)
            .SetDate(date)
            .SetDescription(description)
            .SetResidenceId(residenceId)
            .SetPayingUserId(payingUserId);
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

    public Expense SetPayingUserId(Guid payingUserId)
    {
        if (payingUserId == Guid.Empty) throw new ArgumentException("Usuário pagante inválido.");
        PayingUserId = payingUserId;
        return this;
    }
}
