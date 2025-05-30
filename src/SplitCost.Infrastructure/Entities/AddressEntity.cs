using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Infrastructure.Entities;

[Table("Addresses")]
public class AddressEntity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

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
    public ResidenceEntity Residence { get; private set; } = null!;

    public AddressEntity(){ }
}