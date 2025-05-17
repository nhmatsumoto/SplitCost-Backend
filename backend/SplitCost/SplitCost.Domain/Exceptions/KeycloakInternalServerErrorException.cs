using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakInternalServerErrorException : KeycloakApiException
{
    public KeycloakInternalServerErrorException(string message, string content = null)
        : base(HttpStatusCode.InternalServerError, message, content)
    {
    }
}
