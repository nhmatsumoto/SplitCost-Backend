using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Interfaces
{
    public interface IExpenseRepository
    {
        Task AddAsync(Expense expense);
        Task<Expense?> GetByIdAsync(Guid id);
        Task<IEnumerable<Expense>> GetByResidenceIdAsync(Guid residenceId);
    }
}
