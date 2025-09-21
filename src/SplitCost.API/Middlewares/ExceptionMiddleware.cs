using SplitCost.Domain.Exceptions.Interfaces;
using SplitCost.Domain.Exceptions.Strategy;
using System.Text.Json;

namespace Playground.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware>   _logger;
    private readonly IServiceProvider _serviceProvider;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IServiceProvider serviceProvider)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var scope = _serviceProvider.CreateScope();
        var handlers = scope.ServiceProvider.GetServices<IExceptionHandler>();

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception intercepted by middleware.");

            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                await HandleExceptionAsync(context, ex, handlers);
            }
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, IEnumerable<IExceptionHandler> handlers)
    {
        var handler = handlers.FirstOrDefault(h => h.CanHandle(exception))
                     ?? handlers.First(h => h is DefaultExceptionHandler);

        var (statusCode, message) = handler.Handle(exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        var userMessage = statusCode == 500
            ? "Ocorreu um erro interno. Tente novamente mais tarde."
            : message;

        var errorResponse = new
        {
            Message = userMessage,
            StatusCode = statusCode
        };

        var json = JsonSerializer.Serialize(errorResponse,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(json);

        // Log completo com stack trace
        _logger.LogError(exception, "Exception details: {ExceptionMessage}", exception.Message);
    }

}
