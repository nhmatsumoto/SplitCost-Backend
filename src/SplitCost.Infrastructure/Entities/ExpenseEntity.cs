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

    [Required]
    public ExpenseType Type { get; private set; }

    [Required]
    public ExpenseCategory Category { get; private set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; private set; }

    [Required]
    public DateTime Date { get; private set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; private set; } = null!;

    [Required]
    [ForeignKey("Residence")]
    public Guid ResidenceId { get; set; }

    public ResidenceEntity Residence { get; set; } = null!;

    [Required]
    [ForeignKey("RegisteredBy")]
    public Guid RegisteredByUserId { get; set; }

    public UserEntity RegisteredBy { get; set; } = null!;

    [Required]
    [ForeignKey("PaidBy")]
    public Guid PaidByUserId { get; set; }

    public UserEntity PaidBy { get; set; } = null!;

    [Required]
    public bool IsSharedAmongMembers { get; private set; }
    public ICollection<ExpenseShareEntity> Shares { get; set; } = new List<ExpenseShareEntity>();

    public ExpenseEntity(){ }
}