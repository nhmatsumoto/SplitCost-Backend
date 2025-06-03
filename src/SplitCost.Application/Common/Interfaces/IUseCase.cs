namespace SplitCost.Application.Common.Interfaces;

public interface IUseCase<TInput, TOutput>
{
    Task<TOutput> ExecuteAsync(TInput input, CancellationToken cancellationToken);
}