using SplitCost.Domain.Exceptions.Interfaces;
using System.Net;

namespace SplitCost.Domain.Exceptions.Strategy;

public class NotFoundExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is KeyNotFoundException;

    public (int StatusCode, string Message) Handle(Exception exception) =>
        ((int)HttpStatusCode.NotFound, "The requested resource was not found.");
}
