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
            .MapWith(src => ResidenceFactory.Create(src.Name, src.CreatedByUserId)
                .SetId(src.Id)
                .SetAddressId(src.AddressId));

         //     .Map(src => src.Address.Id, dest => dest.AddressId)
         //   .Map(src => src.Address.Street, dest => dest.Address.Street)
         //   .Map(src => src.Address.Number, dest => dest.Address.Number)
         //   .Map(src => src.Address.Apartment, dest => dest.Address.Apartment)
         //   .Map(src => src.Address.City, dest => dest.Address.City)
         //   .Map(src => src.Address.Prefecture, dest => dest.Address.Prefecture)
         //   .Map(src => src.Address.Country, dest => dest.Address.Country)
         //   .Map(src => src.Address.PostalCode, dest => dest.Address.PostalCode)

    }
}
