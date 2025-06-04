using SplitCost.Domain.Common;
using SplitCost.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Infrastructure.Persistence.Entities;

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

    [Required]
    public string Username { get; set; } = null!;

    public string AvatarUrl { get; set; } = string.Empty;

    public MemberEntity Member { get; set; }

    public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public ICollection<ExpenseEntity> ResidenceExpensesPaid { get; set; } = new List<ExpenseEntity>();


    public UserEntity(){ }
}