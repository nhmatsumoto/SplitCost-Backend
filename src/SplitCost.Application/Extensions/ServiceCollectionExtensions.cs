using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos.AppExpense;
using SplitCost.Application.Dtos.AppIncome;
using SplitCost.Application.Dtos.AppMember;
using SplitCost.Application.Dtos.AppResidence;
using SplitCost.Application.Dtos.AppUser;
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

        // User
        services.AddScoped<IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>, CreateApplicationUserUseCase>();


        // Residence
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

        // Incomes
        services.AddScoped<IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>>, CreateIncomeUseCase>();
        //services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, GetExpenseByIdUseCase>();
        //services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, GetExpensesByResidenceIdUseCase>();

        return services;
    }
}
