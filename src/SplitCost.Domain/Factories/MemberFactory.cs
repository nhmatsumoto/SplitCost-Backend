using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class MemberFactory
{
    /// <summary>
    /// Cria uma instância de Member com os parâmetros fornecidos.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="residenceId"></param>
    /// <param name="joinedAt"></param>
    /// <returns></returns>
    public static Member Create() 
        => new Member();

    public static Member Create(DateTime joinedAt) 
        => new Member()
            .SetJoinedAt(joinedAt);

    public static Member Create(Guid id, Guid userId, Guid residenceId, DateTime joinedAt)
        => new Member()
            .SetId(id)
            .SetUserId(userId)
            .SetResidenceId(residenceId)
            .SetJoinedAt(joinedAt);
}
