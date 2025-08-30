using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakUserNotFoundException : KeycloakApiException
{
    public KeycloakUserNotFoundException(string message, string content = null) : base(HttpStatusCode.InternalServerError, message, content)
    {
    }
}
