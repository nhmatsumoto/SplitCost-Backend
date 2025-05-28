using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Interfaces;
using SplitCost.Infrastructure.Repositories;

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

        services.AddDbContext<SplitCostDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IResidenceRepository, ResidenceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();   
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();  

        //services.AddScoped<IKeycloakService, KeycloakService>();

        return services;
    }
}