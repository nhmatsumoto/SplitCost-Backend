using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.API.Mappers
{
    public class MemberMapperConfig
    {
        public void Register(TypeAdapterConfig config)
        {
            // Input -> Domain
            //config.NewConfig<Member, MemberEntity>()
            //    .Map(dest => dest.Id, src => src.Id)
            //    .Map(dest => dest.UserId, src => src.UserId)
            //    .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            //    .Map(dest => dest.JoinedAt, src => src.JoinedAt);

        }
    }
}
