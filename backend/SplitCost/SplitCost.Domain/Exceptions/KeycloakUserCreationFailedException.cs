using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakUserCreationFailedException : KeycloakApiException
{
    public KeycloakUserCreationFailedException(string message, string content = null)
        : base(HttpStatusCode.InternalServerError, message, content)
    {
    }
}
