namespace SplitCost.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public string? ErrorMessage { get; }
    public ErrorType? ErrorType { get; }
    public object? Data { get; }

    private Result(bool isSuccess, string? errorMessage, object? data = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Data = data;
    }

    public static Result Success(object? data = null) => new Result(true, null, data);
    public static Result Failure(string errorMessage, ErrorType errorType) => new Result(false, errorMessage, errorType);
}
