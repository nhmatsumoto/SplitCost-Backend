using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Mappers;
using SplitCost.Application.UseCases.ApplicationUserUseCases.CreateApplicationUser;
using SplitCost.Application.UseCases.ApplicationUserUseCases.GetApplicationUserById;
using SplitCost.Application.UseCases.ExpenseUseCases.CreateExpense;
using SplitCost.Application.UseCases.GetExpense;
using SplitCost.Application.UseCases.MemberUseCases.AddMember;
using SplitCost.Application.UseCases.MemberUseCases.GetMember;
using SplitCost.Application.UseCases.ResidenceUseCases.CreateResidence;
using SplitCost.Application.UseCases.ResidenceUseCases.GetResidenceById;
using SplitCost.Application.UseCases.ResidenceUseCases.UpdateResidence;

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
        services.AddScoped<IUseCase<GetResidenceByIdInput, Result<GetResidenceByIdOutput>>, GetResidenceByIdUseCase>();
        services.AddScoped<IUseCase<UpdateResidenceInput, Result<UpdateResidenceOutput>>, UpdateResidenceUseCase>();

        // Member
        services.AddScoped<IUseCase<GetMemberByResidenceIdInput, Result<Dictionary<Guid, string>>>, GetMembersByResidenceIdUseCase>();
        services.AddScoped<IUseCase<AddMemberInput, Result<AddMemberOutput>>, AddMemberUseCase>();

        // Expenses
        services.AddScoped<IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>>, CreateExpenseUseCase>();
        services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, GetExpenseByIdUseCase>();
        services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, GetExpensesByResidenceIdUseCase>();

        // Users
        services.AddScoped<IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>, CreateApplicationUserUseCase>();
        services.AddScoped<IUseCase<Guid, Result<GetApplicationUserByIdOutput>>, GetApplicationUserByIdUseCase>();

        // Registra as configurações de mapeamento
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ExpenseMapperConfig).Assembly);

        // Registra IMapper do Mapster
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        // Regira as configurações do Fluent Validation
        services.AddValidatorsFromAssemblyContaining<CreateExpenseInputValidator>();


        return services;
    }
}
