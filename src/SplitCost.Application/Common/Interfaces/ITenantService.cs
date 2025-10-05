namespace SplitCost.Application.Common.Interfaces;

public interface ITenantService
{
    Guid GetCurrentTenantId();
    Guid GetCurrentUserId();

    void SetCurrentTenantId(Guid residenceId);

}
