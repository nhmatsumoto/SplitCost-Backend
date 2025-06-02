using Mapster;
using SplitCost.Application.UseCases.GetResidence;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.Mappers;

public class ResidenceMapperConfig
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Residence, GetResidenceByIdOutput>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.AddressId, src => src.AddressId ?? Guid.Empty)
            .Map(dest => dest.CreatedByUserId, src => src.CreatedByUserId ?? Guid.Empty)
            .Map(dest => dest.Members, src => src.Members)
            .Map(dest => dest.Expenses, src => src.Expenses);
    }
}
