using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Interfaces;
using SplitCost.Application.UseCases;

namespace SplitCost.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICreateResidenceUseCase, CreateResidenceUseCase>();
        services.AddScoped<IUpdateResidenceUseCase, UpdateResidenceUseCase>();
        services.AddScoped<IReadResidenceUseCase, ReadResidenceUseCase>();
        services.AddScoped<IRegisterResidenceOwnerUseCase, RegisterResidenceOwnerUseCase>();
        services.AddScoped<ICreateExpenseUseCase, CreateExpenseUseCase>();
        services.AddScoped<IReadExpenseUseCase, ReadExpenseUseCase>();

        services.AddScoped<IAppUserUseCase, CreateAppUserUseCase>();
        

        return services;
    }
}
