using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SplitCost.Domain.Exceptions.Interfaces;
using SplitCost.Domain.Exceptions.Strategy;

namespace SplitCost.Domain.Exceptions.Extensions;

public static class ExceptionExcetensions
{
    public static IServiceCollection AddExceptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IExceptionHandler, ValidationExceptionHandler>();
        services.AddScoped<IExceptionHandler, DomainExceptionHandler>();
        services.AddScoped<IExceptionHandler, KeycloakExceptionHandler>();
        services.AddScoped<IExceptionHandler, DefaultExceptionHandler>();
        services.AddScoped<IExceptionHandler, UnauthorizedExceptionHandler>();
        services.AddScoped<IExceptionHandler, NotFoundExceptionHandler>();

        return services;
    }
}
