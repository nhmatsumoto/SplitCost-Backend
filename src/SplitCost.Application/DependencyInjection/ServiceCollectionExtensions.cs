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
        services.AddScoped<ICreateResidenceMemberUseCase, CreateResidenceMember>();
        services.AddScoped<ICreateExpenseUseCase, CreateExpenseUseCase>();
        services.AddScoped<IReadExpenseUseCase, ReadExpenseUseCase>();
        services.AddScoped<IReadMemberUseCase, ReadMemberUseCase>();

        services.AddScoped<IAppUserUseCase, CreateAppUserUseCase>();
        

        return services;
    }
}
