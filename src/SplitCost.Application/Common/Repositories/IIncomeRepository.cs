using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories
{
    public interface IIncomeRepository
    {
        Task AddAsync(Income incomeDomain, CancellationToken cancellationToken);
        Task<Income?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
