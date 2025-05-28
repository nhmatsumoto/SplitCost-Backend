using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakForbiddenException : KeycloakApiException
{
    public KeycloakForbiddenException(string message, string content = null)
        : base(HttpStatusCode.Forbidden, message, content)
    {
    }
}