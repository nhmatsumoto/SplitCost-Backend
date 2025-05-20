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
    public ResidenceExpenseType Type { get; set; }
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
}
