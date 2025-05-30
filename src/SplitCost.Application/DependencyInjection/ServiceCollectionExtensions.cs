using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Interfaces;
using SplitCost.Application.Mappers;
using SplitCost.Application.UseCases;

namespace SplitCost.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<ICreateResidenceUseCase, CreateResidenceUseCase>();
        services.AddScoped<IUpdateResidenceUseCase, UpdateResidenceUseCase>();
        services.AddScoped<IReadResidenceUseCase, ReadResidenceUseCase>();
        services.AddScoped<ICreateResidenceMemberUseCase, CreateResidenceMember>();
        services.AddScoped<ICreateExpenseUseCase, CreateExpenseUseCase>();
        services.AddScoped<IReadExpenseUseCase, ReadExpenseUseCase>();
        services.AddScoped<IReadMemberUseCase, ReadMemberUseCase>();
        services.AddScoped<IAppUserUseCase, CreateAppUserUseCase>();

        // Registra as configurações de mapeamento
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ExpenseDomainMapperConfig).Assembly);

        // Registra IMapper do Mapster
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}
