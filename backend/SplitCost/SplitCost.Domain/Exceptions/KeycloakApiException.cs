using System.Net;

namespace SplitCost.Domain.Exceptions;

public class KeycloakApiException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string Content { get; }

    public KeycloakApiException(HttpStatusCode statusCode, string message, string content = null, Exception innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
        Content = content;
    }
}
