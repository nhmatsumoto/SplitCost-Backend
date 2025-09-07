using SplitCost.Domain.Exceptions.Interfaces;
using System.Net;

namespace SplitCost.Domain.Exceptions.Strategy;

public class UnauthorizedExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is UnauthorizedAccessException;

    public (int StatusCode, string Message) Handle(Exception exception) =>
        ((int)HttpStatusCode.Unauthorized, "Unauthorized access.");
}
