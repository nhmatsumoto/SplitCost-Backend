using SplitCost.Application.Common.Interfaces;

namespace SplitCost.Infrastructure.Tenancy;

/// <summary>
/// Implementação de ITenantService para uso em tempo de design.
/// 
/// Essa classe é usada pelo EF Core ao criar migrations, porque não há HttpContext disponível.
/// </summary>
public class DesignTimeTenantService : ITenantService
{
    /// <summary>
    /// Retorna um TenantId fixo para design time.
    /// </summary>
    public Guid GetCurrentTenantId() => Guid.Parse("00000000-0000-0000-0000-000000000001");

    /// <summary>
    /// Retorna um UserId fixo para design time.
    /// </summary>
    public Guid GetCurrentUserId() => Guid.Parse("00000000-0000-0000-0000-000000000002");

    /// <summary>
    /// Não implementado em design time, pois o tenant não pode ser alterado nesse contexto.
    /// </summary>
    public void SetCurrentTenantId(Guid residenceId)
    {
        throw new NotImplementedException("SetCurrentTenantId não é suportado em DesignTimeTenantService.");
    }
}
