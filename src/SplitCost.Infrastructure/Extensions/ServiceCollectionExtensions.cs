using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Services;
using SplitCost.Application.Interfaces;
using SplitCost.Infrastructure.Logging;
using SplitCost.Infrastructure.Repositories;
using SplitCost.Infrastructure.Services;
using System.Net.Http.Headers;

namespace Playground.Infrastructure.DependencyInjection;

/// <summary>
/// Contém métodos de extensão para registrar os serviços da camada de infraestrutura
/// na coleção de dependências da aplicação.
/// 
/// Essa classe isola a configuração de acesso a dados e infraestrutura, mantendo
/// a camada de apresentação desacoplada dos detalhes de implementação.
/// </summary>
public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Registra o <see cref="SplitCostDbContext"/> com SQL Server, utilizando a string de conexão
    /// "DefaultConnection" definida no <see cref="IConfiguration"/>, e adiciona os repositórios
    /// da camada de infraestrutura no container de injeção de dependência.
    /// 
    /// Lança uma exceção se a string de conexão não for encontrada.
    /// </summary>
    /// <param name="services">A coleção de serviços do ASP.NET Core.</param>
    /// <param name="configuration">A configuração da aplicação, usada para recuperar a string de conexão.</param>
    /// <returns>A coleção de serviços atualizada.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");
        }

#warning Alterar base para PostgreSQL
        services.AddDbContext<SplitCostDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IResidenceRepository, ResidenceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IIncomeRepository, IncomeRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUserSettingsRepository, UserSettingsRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork<SplitCostDbContext>>();

        //Keycloak Config
        services.Configure<KeycloakSettings>(configuration.GetSection("Keycloak"));

        services.AddOptions<KeycloakSettings>().Bind(configuration.GetSection("Keycloak")).ValidateOnStart();

        services.AddHttpClient<IKeycloakService, KeycloakService>((serviceProvider, client) =>
        {
            var keycloakSettings = serviceProvider.GetRequiredService<IOptions<KeycloakSettings>>().Value;

            client.BaseAddress = new Uri(keycloakSettings.BaseUrl);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        //Retry policy for HttpClient
        //.AddPolicyHandler(Policy
        //    .Handle<HttpRequestException>()
        //    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
        //);

        // Logging configuration
        services.AddScoped<ILoggerManager, LoggerManager>();





        return services;
    }
}