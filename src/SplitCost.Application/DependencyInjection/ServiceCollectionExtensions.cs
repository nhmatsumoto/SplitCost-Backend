using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Application.Mappers;
using SplitCost.Application.UseCases.CreateApplicationUser;
using SplitCost.Application.UseCases.CreateExpense;
using SplitCost.Application.UseCases.CreateMember;
using SplitCost.Application.UseCases.CreateResidence;
using SplitCost.Application.UseCases.GetApplicationUser;
using SplitCost.Application.UseCases.GetExpense;
using SplitCost.Application.UseCases.GetMember;
using SplitCost.Application.UseCases.GetResidence;
using SplitCost.Application.UseCases.UpdateResidence;
using SplitCost.Application.Validators;

namespace SplitCost.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application services and use cases in the dependency injection container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Residences
        services.AddScoped<IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>>, CreateResidenceUseCase>();

        // TODO
        services.AddScoped<IUpdateResidenceUseCase, UpdateResidenceUseCase>();
        services.AddScoped<IReadResidenceUseCase, GetResidenceByIdUseCase>();

        // Member
        services.AddScoped<IUseCase<Guid, Result<Dictionary<Guid, string>>>, GetMemberByResidenceIdUseCase>();
        services.AddScoped<IUseCase<AddResidenceMemberInput, Result<int>>, AddResidenceMemberUseCase>();

        // Expenses
        services.AddScoped<IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>>, CreateExpenseUseCase>();
        services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, GetExpenseByIdUseCase>();
        services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, GetExpensesByResidenceIdUseCase>();

        // Users
        services.AddScoped<IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>, CreateApplicationUserUseCase>();
        services.AddScoped<IUseCase<Guid, Result<GetApplicationUserByIdOutput>>, GetApplicationUserByIdUseCase>();

        // Registra as configurações de mapeamento
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ExpenseDomainMapperConfig).Assembly);

        // Registra IMapper do Mapster
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        // Regira as configurações do Fluent Validation
        services.AddValidatorsFromAssemblyContaining<CreateExpenseInputValidator>();

        return services;
    }
}
