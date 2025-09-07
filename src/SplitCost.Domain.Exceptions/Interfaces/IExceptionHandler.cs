﻿namespace SplitCost.Domain.Exceptions.Interfaces;

public interface IExceptionHandler
{
    bool CanHandle(Exception exception);
    (int StatusCode, string Message) Handle(Exception exception);
}
