using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly SplitCostDbContext _context;
    private readonly IMapper _mapper;

    public ExpenseRepository(SplitCostDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task AddAsync(Expense expenseDomain, CancellationToken cancellationToken)
    {
        var expenseEntity = _mapper.Map<ExpenseEntity>(expenseDomain);
        await _context.Expenses.AddAsync(expenseEntity, cancellationToken);
    }

    public async Task<Expense?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var expenseEntity = await _context.Expenses.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        return _mapper.Map<Expense>(expenseEntity);
    }

    public async Task<IEnumerable<Expense>> GetByResidenceIdAsync(Guid residenceId, CancellationToken cancellationToken)
    {
        var expensesEntity = await _context.Expenses
            .Where(e => e.ResidenceId == residenceId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<Expense>>(expensesEntity);
    }
       
}
