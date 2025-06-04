using Mapster;
using SplitCost.Application.UseCases.ResidenceUseCases.GetResidenceById;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.Mappers;

public class ResidenceMapperConfig
{
    public void Register(TypeAdapterConfig config)
    {


        config.NewConfig<Residence, GetResidenceByIdOutput>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.CreatedByUserId, src => src.CreatedByUserId)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Street, src => src.Street)
            .Map(dest => dest.Number, src => src.Number)
            .Map(dest => dest.Apartment, src => src.Apartment)
            .Map(dest => dest.City, src => src.City)
            .Map(dest => dest.Prefecture, src => src.Prefecture)
            .Map(dest => dest.Country, src => src.Country)
            .Map(dest => dest.PostalCode, src => src.PostalCode)
            .Map(dest => dest.Members, src => src.Members)
            .Map(dest => dest.Expenses, src => src.Expenses);
    }
}
