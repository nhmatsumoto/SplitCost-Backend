using SplitCost.Domain.Exceptions.Interfaces;
using SplitCost.Domain.Exceptions.Keycloak;
using System.Net;

namespace SplitCost.Domain.Exceptions.Strategy;

public class KeycloakExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception)
        => exception is KeycloakException;

    public (int StatusCode, string Message) Handle(Exception exception)
    {
        var ex = (KeycloakException)exception;

        var statusCode = ex switch
        {
            KeycloakUserConflictException => (int)HttpStatusCode.Conflict,
            KeycloakBadRequestException => (int)HttpStatusCode.BadRequest,
            KeycloakUnauthorizedException => (int)HttpStatusCode.Unauthorized,
            KeycloakForbiddenException => (int)HttpStatusCode.Forbidden,
            KeycloakUserNotFoundException => (int)HttpStatusCode.NotFound,
            KeycloakUserCreationFailedException => (int)HttpStatusCode.InternalServerError,
            _ => (int)HttpStatusCode.InternalServerError
        };

        return (
            StatusCode: statusCode,
            Message: ex.Message
        );
    }
}