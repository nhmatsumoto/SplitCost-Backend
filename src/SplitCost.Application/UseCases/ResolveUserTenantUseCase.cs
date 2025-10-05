using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ResolveUserTenantUseCase
{
    private readonly ITenantService _tenantService;
    private readonly IMemberRepository _memberRepository;
    private readonly IResidenceRepository _residenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResolveUserTenantUseCase(
        ITenantService tenantService,
        IMemberRepository memberRepository,
        IResidenceRepository residenceRepository,
        IUnitOfWork unitOfWork)
    {
        _tenantService = tenantService;
        _memberRepository = memberRepository;
        _residenceRepository = residenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> ResolveAsync(Guid keycloakUserId, string? residenceCode = null, string? newResidenceName = null, CancellationToken cancellation)
    {
        // 1. Verifica se já é membro de alguma residência
        var member = await _memberRepository.GetByExpression(x => x.UserId == keycloakUserId);
        if (member != null)
        {
            _tenantService.SetCurrentTenantId(member.ResidenceId);
            return member.ResidenceId;
        }

        // 2. Criar nova residência
        if (!string.IsNullOrWhiteSpace(newResidenceName))
        {
            //var residence = new Residence(newResidenceName, keycloakUserId, ...);
            //var newMember = new Member(DateTime.UtcNow)
            //    .SetUserId(keycloakUserId)
            //    .SetResidenceId(residence.Id);

            //await _unitOfWork.BeginTransactionAsync(cancellation);
            //await _residenceRepository.AddAsync(residence);
            //await _memberRepository.AddAsync(newMember);
            //await _unitOfWork.CommitAsync(cancellation);

            //_tenantService.SetCurrentTenantId(residence.Id);
            //return residence.Id;
        }

        // 3. Entrar em residência existente via código
        if (!string.IsNullOrWhiteSpace(residenceCode))
        {
            //var residence = await _residenceRepository.GetByCodeAsync(residenceCode);
            //if (residence == null)
            //    throw new InvalidOperationException("Código inválido.");

            //var newMember = new Member(DateTime.UtcNow)
            //    .SetUserId(keycloakUserId)
            //    .SetResidenceId(residence.Id);

            //await _memberRepository.AddAsync(newMember);
            //await _unitOfWork.CommitAsync();

            //_tenantService.SetCurrentTenantId(residence.Id);
            //return residence.Id;
        }

        throw new InvalidOperationException("O usuário precisa criar ou entrar em uma residência.");
    }
}
