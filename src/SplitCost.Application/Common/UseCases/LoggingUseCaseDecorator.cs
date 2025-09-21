using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;

namespace SplitCost.Application.Common.UseCases;

public class LoggingUseCaseDecorator<TInput, TOutput> : IUseCase<TInput, Result<TOutput>>
{
    private readonly IUseCase<TInput, Result<TOutput>> _inner;
    private readonly ILogger<LoggingUseCaseDecorator<TInput, TOutput>> _logger;

    public LoggingUseCaseDecorator(IUseCase<TInput, Result<TOutput>> inner, ILogger<LoggingUseCaseDecorator<TInput, TOutput>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executando use case {UseCase}", typeof(TInput).Name);
        var result = await _inner.ExecuteAsync(input, cancellationToken);
        if (!result.IsSuccess)
            _logger.LogWarning("Use case {UseCase} falhou: {Errors}", typeof(TInput).Name, result.ErrorMessage);
        return result;
    }
}
