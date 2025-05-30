using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class ReadMemberUseCase : IReadMemberUseCase
{
    private readonly IMemberRepository _memberRepository;
    public ReadMemberUseCase(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository ?? throw new ArgumentException(nameof(memberRepository));
    }

    // Retornar objeto mapeado para um DTO, não expor a entidade diretamente
    public async Task<Result<Dictionary<Guid, string>>> GetByResidenceIdAsync(Guid residenceId)
    {
        if (residenceId == Guid.Empty)
            return Result<Dictionary<Guid, string>>.Failure("A residência informada é inválida.", ErrorType.Validation);

        var members = await _memberRepository.GetUsersByResidenceId(residenceId);

        if (members is null || !members.Any())
            return Result<Dictionary<Guid, string>>.Failure("Nenhum membro encontrado para esta residência.", ErrorType.NotFound);

        return Result<Dictionary<Guid, string>>.Success(members);
    }
}

