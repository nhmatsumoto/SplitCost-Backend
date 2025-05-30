namespace SplitCost.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public string? ErrorMessage { get; }
    public ErrorType? ErrorType { get; }
    public object? Data { get; }
    public Dictionary<string, string[]>? ValidationErrors { get; }

    private Result(bool isSuccess, string? errorMessage, ErrorType? errorType, object? data = null, Dictionary<string, string[]>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public static Result Success(object? data = null) => new Result(true, null, null, data, null);
    public static Result Failure(string errorMessage, ErrorType errorType) 
        => new Result(false, errorMessage, errorType, null, null);
    public static Result Failure(string errorMessage, ErrorType errorType, Dictionary<string, string[]> validationErrors) 
        => new Result(false, errorMessage, errorType, null, validationErrors);
    public static Result Invalid(string errorMessage, ErrorType errorType, Dictionary<string, string[]> validationErrors) 
        => new Result(false, errorMessage, errorType, null, validationErrors);
}
