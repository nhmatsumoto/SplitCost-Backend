using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Infrastructure.Entities;

[Table("ExpenseShares")]
public class ExpenseShareEntity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("Expense")]
    [Column("ExpenseId")]
    public Guid ExpenseId { get; set; }
    public ExpenseEntity Expense { get; set; }

    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; set; }
    public UserEntity User { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    public ExpenseShareEntity() { }
}
