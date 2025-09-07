using FluentValidation;
using SplitCost.Domain.Exceptions.Interfaces;
using System.Net;

namespace SplitCost.Domain.Exceptions.Strategy;

public class ValidationExceptionHandler : IExceptionHandler
{
    public bool CanHandle(Exception exception) => exception is ValidationException;

    public (int StatusCode, string Message) Handle(Exception exception)
    {
        var validationEx = (ValidationException)exception;
        return (
           (int)HttpStatusCode.BadRequest,
           $"Validation failed. Check the request data: {validationEx.Errors.Select(e => e.ErrorMessage).ToList()}"
       );
    }
}
