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
    public static Member Create(
        Guid userId, 
        Guid residenceId, 
        DateTime joinedAt)
    {
        return new Member(userId, residenceId, joinedAt);
    }
}
