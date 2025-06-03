using SplitCost.Domain.Common;
using SplitCost.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Infrastructure.Entities;

[Table("Expenses")]
public class ExpenseEntity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }
    public ExpenseType Type { get; private set; }
    public ExpenseCategory Category { get; private set; }
    public DateTime Date { get; private set; }
    public bool IsSharedAmongMembers { get; private set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; private set; }

    [MaxLength(500)]
    [Required]
    public string Description { get; private set; } = null!;

    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; set; }
    public ResidenceEntity Residence { get; set; } = null!;

    [ForeignKey("RegisteredBy")]
    [Column("RegisteredByUserId")]
    public Guid RegisteredByUserId { get; set; }
    public UserEntity RegisteredBy { get; set; } = null!;

    [ForeignKey("PaidBy")]
    public Guid PaidByUserId { get; set; }
    public UserEntity PaidBy { get; set; } = null!;

    public ExpenseEntity(){ }
}