using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Mappers;

public class ResidenceEntityMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain -> Entity
        config.NewConfig<Residence, ResidenceEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.AddressId, src => src.AddressId)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt);

        // Entity -> Domain
        config.NewConfig<ResidenceEntity, Residence>()
            .MapWith(src => ResidenceFactory.Create(
                src.Name,
                src.CreatedByUserId));
    }
}
