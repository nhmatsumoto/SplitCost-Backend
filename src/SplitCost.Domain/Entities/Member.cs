using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class Member : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid ResidenceId { get; private set; }
    public Residence Residence { get; private set; }
    public DateTime JoinedAt { get; private set; }

    internal Member() { }

    internal Member(Guid userId, Guid residenceId, DateTime joinedAt)
    {
        SetUserId(userId);
        SetResidenceId(residenceId);
        SetJoinedAt(joinedAt);
    }

    public Member SetUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Usuário inválido.");
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
