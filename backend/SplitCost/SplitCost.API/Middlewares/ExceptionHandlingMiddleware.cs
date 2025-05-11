using FluentValidation;
using System.Net;
using System.Text.Json;

namespace Playground.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception was caught by the middleware.");

                context.Response.Clear();
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (context.Response.HasStarted)
            {
                return; 
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ValidationException => (int)HttpStatusCode.BadRequest,
                ArgumentNullException => (int)HttpStatusCode.BadRequest,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var errorResponse = new
            {
                Message = exception switch
                {
                    ValidationException => "Validation failed. Check the request data.",
                    ArgumentNullException argEx => $"Argument '{argEx.ParamName}' cannot be null.",
                    KeyNotFoundException => "The requested resource was not found.",
                    UnauthorizedAccessException => "Unauthorized access.",
                    _ => "An unexpected error occurred."
                },
                StatusCode = context.Response.StatusCode,
                Details = exception is ValidationException validationException
                    ? validationException.Errors.Select(e => e.ErrorMessage).ToList()
                    : null
            };

            var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(json);
        }
    }
}
