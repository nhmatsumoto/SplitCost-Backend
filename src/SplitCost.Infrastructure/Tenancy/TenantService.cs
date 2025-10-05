using Microsoft.AspNetCore.Http;
using SplitCost.Application.Common.Interfaces;
using System.Security.Claims;

namespace SplitCost.Infrastructure.Tenancy;

public class TenantService : ITenantService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _currentTenantId;

    public TenantService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentTenantId()
    {
        // Se já foi setado manualmente, retorna o valor
        if (_currentTenantId.HasValue)
            return _currentTenantId.Value;

        // Caso contrário, tenta ler do claim do token
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("residence_id");
        if (claim == null)
            throw new UnauthorizedAccessException("ResidenceId não encontrado no token.");

        return Guid.Parse(claim.Value);
    }


    /// <summary>
    /// Retorna o UserId atual (UUID do Keycloak) a partir do token JWT.
    /// </summary>
    public Guid GetCurrentUserId()
    {
        // "sub" é o claim padrão do Keycloak para identificar o usuário
        var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)
                    ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub");

        if (claim == null)
            throw new UnauthorizedAccessException("UserId não encontrado no token.");

        return Guid.Parse(claim.Value);
    }

    public void SetCurrentTenantId(Guid residenceId)
    {
        if (residenceId == Guid.Empty)
            throw new ArgumentException("ResidenceId inválido.", nameof(residenceId));

        _currentTenantId = residenceId;
    }

}
