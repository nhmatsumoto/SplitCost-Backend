using SplitCost.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SplitCost.Domain.Entities;

[Table("Members")]
public class Member : BaseEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; private set; }

    [ForeignKey("User")]
    [Column("UserId")]
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    [ForeignKey("Residence")]
    [Column("ResidenceId")]
    public Guid ResidenceId { get; private set; }
    public Residence Residence { get; private set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Member()
    {
        
    }

    public Member(Guid userId, Guid residenceId, DateTime joinedAt)
    {
        SetUserId(userId);
        SetResidenceId(residenceId);
        SetJoinedAt(joinedAt);
    }

    public Member SetUserId(Guid userId)
    {
        if (userId == Guid.Empty) 
            throw new ArgumentNullException("É necessário informar um usuário");
        UserId = userId;
        return this;
    }

    public Member SetResidenceId(Guid residenceId)
    {
        if (residenceId == Guid.Empty) 
            throw new ArgumentNullException("É necessário informar uma residência");
        ResidenceId = residenceId;
        return this;
    }

    public Member SetJoinedAt(DateTime joinedAt)
    {
        JoinedAt = joinedAt;
        return this;
    }

}
