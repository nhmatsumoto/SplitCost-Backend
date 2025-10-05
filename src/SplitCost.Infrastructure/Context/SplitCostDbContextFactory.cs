using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SplitCost.Infrastructure.Context;
using SplitCost.Infrastructure.Tenancy;

namespace Playground.Infrastructure.Context;

/// <summary>
/// Fábrica utilizada pelo Entity Framework Core em tempo de design
/// para criar instâncias do <see cref="SplitCostDbContext"/>.
/// 
/// Essa classe é usada automaticamente ao executar comandos CLI do EF,
/// como <c>dotnet ef migrations add</c> ou <c>dotnet ef database update</c>,
/// quando a configuração do DbContext não está no projeto de inicialização (Presentation).
/// 
/// A fábrica lê as configurações do arquivo <c>appsettings.json</c> localizado
/// no diretório atual (Infrastructure), e configura o DbContext com a string de conexão
/// definida na chave <c>ConnectionStrings:DefaultConnection</c>.
/// </summary>
public class SplitCostDbContextFactory : IDesignTimeDbContextFactory<SplitCostDbContext>
{
    public SplitCostDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<SplitCostDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        var tenantService = new DesignTimeTenantService();

        return new SplitCostDbContext(optionsBuilder.Options, tenantService);
    }
}
