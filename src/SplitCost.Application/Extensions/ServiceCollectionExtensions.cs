using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;
using SplitCost.Application.UseCases;
using SplitCost.Application.Validations;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application services and use cases in the dependency injection container.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // FluentValidation Configuration
        services.AddValidatorsFromAssemblyContaining<CreateExpenseInputValidator>();

        // Residences
        services.AddScoped<IUseCase<CreateResidenceInput, Result<Residence>>, CreateResidenceUseCase>();
        services.AddScoped<IUseCase<GetResidenceByIdInput, Result<Residence>>, ReadResidenceByIdUseCase>();
        services.AddScoped<IUseCase<UpdateResidenceInput, Result<UpdateResidenceOutput>>, UpdateResidenceUseCase>();

        // Member
        services.AddScoped<IUseCase<GetMemberByResidenceIdInput, Result<IEnumerable<MemberItemDto>>>, ReadMembersByResidenceIdUseCase>();
        services.AddScoped<IUseCase<AddMemberInput, Result<Member>>, CreateMemberUseCase>();

        // Expenses
        services.AddScoped<IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>>, CreateExpenseUseCase>();
        services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, ReadExpenseByIdUseCase>();
        services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, ReadExpensesByResidenceIdUseCase>();

        // Users
        services.AddScoped<IUseCase<CreateUserInput, Result<CreateUserOutput>>, CreateApplicationUserUseCase>();
        services.AddScoped<IUseCase<Guid, Result<User>>, GetApplicationUserByIdUseCase>();
        services.AddScoped<IUseCase<Guid, Result<UserSettings>>, ReadUserSettingsByUserIdUseCase>();


        // Incomes
        services.AddScoped<IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>>, CreateIncomeUseCase>();
        //services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, GetExpenseByIdUseCase>();
        //services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, GetExpensesByResidenceIdUseCase>();

        return services;
    }
}
