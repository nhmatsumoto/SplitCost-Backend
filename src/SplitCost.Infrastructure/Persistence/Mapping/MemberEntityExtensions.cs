using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Persistence.Mapping;

public static class MemberEntityExtensions
{
    /// <summary>
    /// Converte uma instância de MemberEntity para uma instância de Member do domínio.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static Member ToDomain(this MemberEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return MemberFactory
            .Create()
            .SetId(entity.Id)
            .SetUserId(entity.UserId)
            .SetResidenceId(entity.ResidenceId)
            .SetJoinedAt(entity.JoinedAt);
    }

    /// <summary>
    /// Converte uma instância de Member do domínio para uma instância de MemberEntity.
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static MemberEntity ToEntity(this Domain.Entities.Member member)
    {
        if (member == null) throw new ArgumentNullException(nameof(member));

        return new MemberEntity
        {
            Id = member.Id,
            UserId = member.UserId,
            ResidenceId = member.ResidenceId
        };
    }

}
