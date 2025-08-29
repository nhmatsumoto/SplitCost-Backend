using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.Common.Configuration;

public static class MapsterConfig
{
    public static void ConfigureMappings()
    {
        // Configuração global do Mapster
        TypeAdapterConfig.GlobalSettings.Default
            .PreserveReference(true) // Preserva referências circulares, se necessário
            .IgnoreNullValues(true); // Ignora valores nulos durante o mapeamento

        // Mapeamento específico para Income
        TypeAdapterConfig<CreateIncomeInput, Income>.NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid()) // Exemplo: Gerar novo ID
            .Map(dest => dest.CreatedAt, src => DateTime.UtcNow) // Exemplo: Definir data de criação
            .Ignore(dest => dest.UpdatedAt); // Ignorar propriedades não mapeadas

        TypeAdapterConfig<CreateIncomeInput, Income>.NewConfig()
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Description, src => src.Description) // Removido ?? "", pois SetDescription já valida
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.ResidenceId, src => src.ResidenceId);


        // Input -> Domain
        TypeAdapterConfig<CreateExpenseInput, Expense>.NewConfig()
            .MapWith(src => ExpenseFactory.Create(
                src.Type,
                src.Category,
                src.Amount,
                src.Date,
                src.Description,
                src.IsSharedAmongMembers,
                src.ResidenceId,
                src.RegisteredByUserId,
                src.PaidByUserId
            ));

        // Domain -> Output
        TypeAdapterConfig<Expense, CreateExpenseOutput>.NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Type, src => src.Type)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Date, src => src.Date)
            .Map(dest => dest.Description, src => src.Description ?? "")
            .Map(dest => dest.ResidenceId, src => src.ResidenceId)
            .Map(dest => dest.RegisteredByUserId, src => src.RegisteredByUserId)
            .Map(dest => dest.PaidByUserId, src => src.PaidByUserId);


    }

    // Método para registrar o Mapster no contêiner de DI
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        ConfigureMappings();
        services.AddSingleton<IMapper, ServiceMapper>();
        return services;
    }
}
