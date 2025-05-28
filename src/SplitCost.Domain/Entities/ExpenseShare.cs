using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Domain.Entities;

public class ExpenseShare : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [ForeignKey("Expense")]
    [Column("ExpenseId")]
    public Guid ExpenseId { get; private set; }
    public Expense Expense { get; private set; }

    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    [Column("Amount")]
    public decimal Amount { get; private set; }

    public ExpenseShare()
    {
        
    }

    public ExpenseShare SetAmount(decimal amount)
    {
        if(amount < 0) throw new ArgumentOutOfRangeException("O valor da despesa deve ser maior que zero."); 
        this.Amount = amount;
        return this;
    }
}
