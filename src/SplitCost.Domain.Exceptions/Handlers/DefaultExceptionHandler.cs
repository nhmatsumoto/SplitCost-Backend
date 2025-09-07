using SplitCost.Domain.Exceptions.Interfaces;
using System.Net;

namespace SplitCost.Domain.Exceptions.Strategy;

public class DefaultExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => true;

    public (int StatusCode, string Message) Handle(Exception exception) =>
        ((int)HttpStatusCode.InternalServerError, $"An unexpected error occurred {exception.Message}");
}
