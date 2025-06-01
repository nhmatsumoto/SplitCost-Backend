using FluentValidation;
using SplitCost.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Playground.API.Middlewares;

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
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
            KeycloakUserConflictException => (int)HttpStatusCode.Conflict,
            KeycloakBadRequestException => (int)HttpStatusCode.BadRequest,
            KeycloakUnauthorizedException => (int)HttpStatusCode.Unauthorized,
            KeycloakForbiddenException => (int)HttpStatusCode.Forbidden,
            KeycloakInternalServerErrorException => (int)HttpStatusCode.InternalServerError,
            KeycloakUserCreationFailedException => (int)HttpStatusCode.InternalServerError,
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
                KeycloakUserConflictException keycloakConflictEx => keycloakConflictEx.Message,
                KeycloakBadRequestException keycloakBadRequestEx => keycloakBadRequestEx.Message,
                KeycloakUnauthorizedException keycloakUnauthorizedEx => keycloakUnauthorizedEx.Message,
                KeycloakForbiddenException keycloakForbiddenEx => keycloakForbiddenEx.Message,
                KeycloakInternalServerErrorException keycloakInternalServerErrorEx => keycloakInternalServerErrorEx.Message,
                KeycloakUserCreationFailedException keycloakCreationFailedEx => keycloakCreationFailedEx.Message,
                _ => "An unexpected error occurred."
            },
            StatusCode = context.Response.StatusCode,
            Details = GetExceptionDetails(exception)
        };

        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(json);
    }

    private static object GetExceptionDetails(Exception exception)
    {
        if (exception is ValidationException validationException)
        {
            return validationException.Errors.Select(e => e.ErrorMessage).ToList();
        }
        else if (exception is KeycloakApiException keycloakEx)
        {
            return keycloakEx.Content;
        }
        return null;
    }
}
