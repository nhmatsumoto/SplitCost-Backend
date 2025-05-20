using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Domain.Entities;

public class ResidenceExpenseShare : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("ResidenceExpense")]
    [Column("ResidenceExpenseId")]
    public Guid ResidenceExpenseId { get; set; }
    public Expense ResidenceExpense { get; set; } = null!;

    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public decimal Amount { get; set; }
}
