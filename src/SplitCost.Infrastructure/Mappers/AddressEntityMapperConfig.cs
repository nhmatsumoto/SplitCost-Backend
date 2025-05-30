using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Mappers;
public class AddressEntityMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain -> Entity
        config.NewConfig<Address, AddressEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Apartment, src => src.Apartment ?? "")
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Prefecture, src => src.Prefecture)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.PostalCode, src => src.PostalCode);

        // Entity -> Domain
        config.NewConfig<AddressEntity, Address>()
            .MapWith(src => AddressFactory.Create(
                src.Street,
                src.Number,
                src.Apartment ?? string.Empty,
                src.City,
                src.Prefecture,
                src.Country,
                src.PostalCode
            ));
    }
}
