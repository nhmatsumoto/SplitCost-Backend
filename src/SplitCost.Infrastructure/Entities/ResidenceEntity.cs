using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Infrastructure.Entities;

[Table("Residences")]
public class ResidenceEntity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(200)]
    [Column("Name")]
    public string Name { get; set; }

    [ForeignKey("Address")]
    [Column("AddressId")]
    public Guid AddressId { get; set; }

    public AddressEntity Address { get; set; }

    [ForeignKey("CreatedBy")]
    [Column("CreatedByUserId")]
    public Guid CreatedByUserId { get; set; }

    public UserEntity CreatedBy { get; set; }

    public ICollection<MemberEntity> Members { get; set; } = new List<MemberEntity>();

    public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public ResidenceEntity() { }
}
