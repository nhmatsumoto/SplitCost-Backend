using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakBadRequestException : KeycloakApiException
{
    public KeycloakBadRequestException(string message, string content = null)
        : base(HttpStatusCode.BadRequest, message, content)
    {
    }
}
