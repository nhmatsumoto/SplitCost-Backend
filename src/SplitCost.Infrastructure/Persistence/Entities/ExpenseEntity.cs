using SplitCost.Domain.Common;
using SplitCost.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Infrastructure.Persistence.Entities;

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
    public Guid ResidenceId { get; private set; }
    public ResidenceEntity Residence { get; private set; } = null!;

    [ForeignKey("RegisteredBy")]
    [Column("RegisteredByUserId")]
    public Guid RegisteredByUserId { get; private set; }
    public UserEntity RegisteredBy { get; private set; } = null!;

    [ForeignKey("PaidBy")]
    public Guid PaidByUserId { get; private set; }
    public UserEntity PaidBy { get; private set; } = null!;

    public ExpenseEntity(){ }
}