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

    [ForeignKey("CreatedBy")]
    [Column("CreatedByUserId")]
    public Guid CreatedByUserId { get; set; }
    public UserEntity CreatedBy { get; set; }

    //Address
    [Required]
    [MaxLength(200)]
    public string Street { get; private set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Number { get; private set; } = null!;

    [MaxLength(50)]
    public string Apartment { get; private set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string City { get; private set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Prefecture { get; private set; } = null!;

    [Required]
    [MaxLength(100)]
    public string Country { get; private set; } = null!;

    [Required]
    [MaxLength(20)]
    public string PostalCode { get; private set; } = null!;

    public ICollection<MemberEntity> Members { get; set; } = new List<MemberEntity>();

    public ICollection<ExpenseEntity> Expenses { get; set; } = new List<ExpenseEntity>();

    public ResidenceEntity() { }
}
