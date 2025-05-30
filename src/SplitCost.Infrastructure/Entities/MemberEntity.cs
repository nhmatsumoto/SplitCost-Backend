using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Infrastructure.Entities;

[Table("Members")]
public class MemberEntity : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    [Required]
    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; set; }
    public ResidenceEntity Residence { get; set; } = null!;

    [Required]
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public MemberEntity(){ }
}
