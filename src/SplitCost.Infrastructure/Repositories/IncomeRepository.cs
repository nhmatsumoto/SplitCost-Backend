using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Repositories
{
    public class IncomeRepository : IIncomeRepository
    {
        private readonly SplitCostDbContext _context;
        private readonly IMapper _mapper;

        public IncomeRepository(SplitCostDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task AddAsync(Income incomeDomain, CancellationToken cancellationToken)
        {
            var incomeEntity = _mapper.Map<IncomeEntity>(incomeDomain);
            await _context.Incomes.AddAsync(incomeEntity, cancellationToken);
        }
        public async Task<Income?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var incomeEntity = await _context.Incomes.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
            return _mapper.Map<Income>(incomeEntity);
        }
        

        //public async Task<IEnumerable<Income>> GetByResidenceIdAsync(Guid residenceId, CancellationToken cancellationToken)
        //{
        //    var incomeEntity = await _context.Incomes
        //        .Where(e => e.ResidenceId == residenceId)
        //        .ToListAsync(cancellationToken);

        //    return _mapper.Map<IEnumerable<Income>>(incomeEntity);
        //}

    }
}
