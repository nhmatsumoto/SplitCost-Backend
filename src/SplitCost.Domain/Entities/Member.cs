using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplitCost.Domain.Entities;

[Table("Members")]
public class Member : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    [Required]
    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; set; }
    public Residence Residence { get; set; } = null!;

    [Required]
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    internal Member() { }

    //Guid userId, Guid residenceId, 
    internal Member(DateTime joinedAt)
    {
        //SetUserId(userId);
        //SetResidenceId(residenceId);
        SetJoinedAt(joinedAt);
    }

    public Member SetId(Guid id) 
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.");
        Id = id;
        return this;
    }
    public Member SetUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId inválido.");
        UserId = userId;
        return this;
    }

    public Member SetResidenceId(Guid residenceId)
    {
        if (residenceId == Guid.Empty)
            throw new ArgumentException("Residência inválida.");
        ResidenceId = residenceId;
        return this;
    }

    public Member SetJoinedAt(DateTime joinedAt)
    {
        if (joinedAt == default)
            throw new ArgumentException("Data de entrada inválida.");
        JoinedAt = joinedAt;
        return this;
    }
}
