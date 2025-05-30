using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Infrastructure.Entities;

[Table("Users")]
public class UserEntity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public string AvatarUrl { get; set; } = string.Empty;

    // Navigation properties

    public ICollection<MemberEntity> Residences { get; set; } = new List<MemberEntity>();

    public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public ICollection<ExpenseEntity> ResidenceExpensesPaid { get; set; } = new List<ExpenseEntity>();

    public ICollection<ExpenseShareEntity> ExpenseShares { get; set; } = new List<ExpenseShareEntity>();

    public UserEntity(){ }
}