using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Application.Common.Configuration;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.UseCases;
using SplitCost.Application.UseCases.Dtos;
using SplitCost.Application.UseCases.Validations;
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
        // Mapster Configuration
        TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfig).Assembly);
        services.AddScoped<IMapper, ServiceMapper>();

        // FluentValidation Configuration
        services.AddValidatorsFromAssemblyContaining<CreateExpenseInputValidator>();

        // Residences
        services.AddScoped<IUseCase<CreateResidenceInput, Result<Residence>>, CreateResidenceUseCase>();
        services.AddScoped<IUseCase<GetResidenceByIdInput, Result<Residence>>, ReadResidenceByIdUseCase>();
        services.AddScoped<IUseCase<UpdateResidenceInput, Result<UpdateResidenceOutput>>, UpdateResidenceUseCase>();

        // Member
        services.AddScoped<IUseCase<GetMemberByResidenceIdInput, Result<Dictionary<Guid, string>>>, ReadMembersByResidenceIdUseCase>();
        services.AddScoped<IUseCase<AddMemberInput, Result<Member>>, CreateMemberUseCase>();

        // Expenses
        services.AddScoped<IUseCase<CreateExpenseInput, Result<CreateExpenseOutput>>, CreateExpenseUseCase>();
        services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, ReadExpenseByIdUseCase>();
        services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, ReadExpensesByResidenceIdUseCase>();

        // Users
        services.AddScoped<IUseCase<CreateApplicationUserInput, Result<CreateApplicationUserOutput>>, CreateApplicationUserUseCase>();
        services.AddScoped<IUseCase<Guid, Result<User>>, GetApplicationUserByIdUseCase>();
        services.AddScoped<IUseCase<Guid, Result<UserSettings>>, ReadUserSettingsByUserIdUseCase>();




        // Incomes
        services.AddScoped<IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>>, CreateIncomeUseCase>();
        //services.AddScoped<IUseCase<Guid, Result<GetExpenseByIdOutput>>, GetExpenseByIdUseCase>();
        //services.AddScoped<IUseCase<Guid, Result<IEnumerable<GetExpenseByIdOutput>>>, GetExpensesByResidenceIdUseCase>();

        return services;
    }
}
