using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Responses;

namespace SplitCost.Application.Common.UseCases;

public abstract class BaseUseCase<TInput, TOutput> : IUseCase<TInput, Result<TOutput>>
{
    public async Task<Result<TOutput>> ExecuteAsync(TInput input, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await ValidateAsync(input, cancellationToken);
            if (!validationResult.IsValid)
                return Result<TOutput>.FromFluentValidation("Erro de validação", validationResult.Errors);

            return await HandleAsync(input, cancellationToken);
        }
        catch (Exception ex)
        {
            return Result<TOutput>.Failure($"Erro inesperado: {ex.Message}", ErrorType.InternalError);
        }
    }

    protected virtual Task<FluentValidation.Results.ValidationResult> ValidateAsync(TInput input, CancellationToken cancellationToken)
        => Task.FromResult(new FluentValidation.Results.ValidationResult());

    protected abstract Task<Result<TOutput>> HandleAsync(TInput input, CancellationToken cancellationToken);
}
