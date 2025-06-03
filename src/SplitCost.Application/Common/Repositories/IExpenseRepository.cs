using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories;

public interface IExpenseRepository
{
    Task AddAsync(Expense expense, CancellationToken cancellationToken);
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<Expense>> GetByResidenceIdAsync(Guid residenceId, CancellationToken cancellationToken);
}
