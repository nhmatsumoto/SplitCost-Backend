using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly SplitCostDbContext _context;

    public ExpenseRepository(SplitCostDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        //await _context.SaveChangesAsync(); Não deve ser chamado aqui   
    }

    public async Task<Expense?> GetByIdAsync(Guid id) =>
        await _context.Expenses.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<IEnumerable<Expense>> GetByResidenceIdAsync(Guid residenceId) =>
        await _context.Expenses
            .Where(e => e.ResidenceId == residenceId)
            .ToListAsync();
}
