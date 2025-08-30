using SpitCost.Infrastructure.Context;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;

namespace SplitCost.Infrastructure.Repositories;

public class UserSettingsRepository : Repository<UserSettings>, IUserSettingsRepository
{
    private readonly SplitCostDbContext _context;
    public UserSettingsRepository(SplitCostDbContext context) : base(context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

}
