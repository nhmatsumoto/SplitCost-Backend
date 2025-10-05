using SplitCost.Application.Common.Interfaces;

namespace SplitCost.Infrastructure.Tenancy;

public class DesignTimeTenantService : ITenantService
{
    public Guid GetCurrentTenantId() => Guid.Parse("00000000-0000-0000-0000-000000000001");
    public Guid GetCurrentUserId() => Guid.Parse("00000000-0000-0000-0000-000000000002");
}
