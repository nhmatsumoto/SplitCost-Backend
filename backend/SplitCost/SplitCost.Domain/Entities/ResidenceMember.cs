using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Domain.Entities;

public class ResidenceMember : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
