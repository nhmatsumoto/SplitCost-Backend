using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.Common.Repositories;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<List<Expense>> GetExpenseListByResidenceId(Guid residenceId, CancellationToken cancellationToken);
}
