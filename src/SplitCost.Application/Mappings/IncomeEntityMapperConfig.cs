using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Mappings
{
    public class IncomeEntityMapperConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // Domain -> Entity
            // Ignora propriedades de navegação para evitar mapeamento automático

            config.NewConfig<Income, IncomeEntity>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Amount, src => src.Amount)
                .Map(dest => dest.Category, src => src.Category)
                .Map(dest => dest.Description, src => src.Description) // Removido ?? "", pois SetDescription já valida
                .Map(dest => dest.RegisteredByUserId, src => src.RegisteredByUserId)
                .Map(dest => dest.ResidenceId, src => src.ResidenceId)
                .Map(dest => dest.Date, src => src.Date)
                .Ignore(dest => dest.RegisteredByUser)
                .Ignore(dest => dest.Residence);

            //// Entity -> Domain
            //config.NewConfig<IncomeEntity, Income>()
            //    .ConstructUsing(src => IncomeFactory.Create(
            //        amount: src.Amount,
            //        category: src.Category,
            //        date: src.Date,
            //        description: src.Description ?? string.Empty, // Garante que Description não seja null
            //        residenceId: src.ResidenceId)
            //    .Map(dest => dest.Id, src => src.Id)
            //    // Ignora propriedades de navegação para evitar mapeamento automático
            //    .Ignore(dest => dest.Residence)
            //    .Ignore(dest => dest.RegisteredBy);
        }
    }
}
