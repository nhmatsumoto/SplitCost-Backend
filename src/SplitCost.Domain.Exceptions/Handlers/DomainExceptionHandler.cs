using SplitCost.Domain.Exceptions.Common;
using SplitCost.Domain.Exceptions.Interfaces;
using System.Net;

namespace SplitCost.Domain.Exceptions.Strategy;

public class DomainExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is DomainException;

    public (int StatusCode, string Message) Handle(Exception exception)
    {
        if (exception is not DomainException domainEx)
            throw new ArgumentException("Exception must be of type DomainException", nameof(exception));

        return (
            (int)HttpStatusCode.BadRequest,      // Status code adequado para erro de negócio
            domainEx.Message                     // Mensagem clara para o cliente
        );
    }
}
