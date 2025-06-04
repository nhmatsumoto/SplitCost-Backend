using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Mappers;
public class MemberEntityMapperConfig : IRegister
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
        //src.Id, src.ResidenceId,
        config.NewConfig<MemberEntity, Member>()
            .MapWith(src => Domain.Factories.MemberFactory.Create(src.JoinedAt));
    }
}
