using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Mappers;
public class MemberEntityMapperConfiguration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain -> Entity
        config.NewConfig<Member, MemberEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.JoinedAt, src => src.JoinedAt);

        // Entity -> Domain
        config.NewConfig<Entities.MemberEntity, Domain.Entities.Member>()
            .MapWith(src => Domain.Factories.MemberFactory.Create(
                src.Id,
                src.ResidenceId,
                src.JoinedAt));
    }
}
