using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Mappers;

public class ResidenceEntityMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain -> Entity
        config.NewConfig<Residence, ResidenceEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CreatedByUserId, src => src.CreatedByUserId)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Apartment, src => src.Apartment)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Prefecture, src => src.Prefecture)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.PostalCode, src => src.PostalCode)
            .Map(dest => dest.CreatedAt, src => src.CreatedAt)
            .Map(dest => dest.UpdatedAt, src => src.UpdatedAt);

        // Entity -> Domain
        config.NewConfig<ResidenceEntity, Residence>().MapWith(src => ResidenceFactory
            .Create()
            .SetId(src.Id)
            .SetName(src.Name)
            .SetCreatedByUser(src.CreatedByUserId)
            .SetStreet(src.Street)
            .SetNumber(src.Number)
            .SetApartment(src.Apartment)
            .SetCity(src.City)
            .SetPrefecture(src.Prefecture)
            .SetCountry(src.Country)
            .SetPostalCode(src.PostalCode)
        );

    }
}
