using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakUserConflictException : KeycloakApiException
{
    public KeycloakUserConflictException(string message, string content = null)
        : base(HttpStatusCode.Conflict, message, content)
    {
    }
}