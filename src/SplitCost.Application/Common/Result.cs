namespace SplitCost.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;
    public string? ErrorMessage { get; }
    public ErrorType? ErrorType { get; }
    public T? Data { get; }
    public Dictionary<string, string[]>? ValidationErrors { get; }

    private Result(bool isSuccess, string? errorMessage, ErrorType? errorType, T? data = default, Dictionary<string, string[]>? validationErrors = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
        Data = data;
        ValidationErrors = validationErrors;
    }

    public static Result<T> Success(T? data = default) =>
        new Result<T>(true, null, null, data, null);

    public static Result<T> Failure(string errorMessage, ErrorType errorType) =>
        new Result<T>(false, errorMessage, errorType);

    public static Result<T> Failure(string errorMessage, ErrorType errorType, Dictionary<string, string[]> validationErrors) =>
        new Result<T>(false, errorMessage, errorType, default, validationErrors);

    public static Result<T> Invalid(string errorMessage, ErrorType errorType, Dictionary<string, string[]> validationErrors) =>
        new Result<T>(false, errorMessage, errorType, default, validationErrors);
}
