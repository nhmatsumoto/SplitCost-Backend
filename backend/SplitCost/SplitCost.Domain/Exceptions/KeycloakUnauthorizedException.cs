using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakUnauthorizedException : KeycloakApiException
{
    public KeycloakUnauthorizedException(string message, string content = null)
        : base(HttpStatusCode.Unauthorized, message, content)
    {
    }
}