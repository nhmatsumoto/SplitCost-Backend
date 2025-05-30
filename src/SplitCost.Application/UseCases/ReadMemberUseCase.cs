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
    public async Task<Result> GetByResidenceIdAsync(Guid residenceId)
    {
        if (residenceId == Guid.Empty)
            return Result.Failure("A residência informada é inválida.", ErrorType.Validation);

        var members = await _memberRepository.GetUsersByResidenceId(residenceId);

        if (members is null || !members.Any())
            return Result.Failure("Nenhum membro encontrado para esta residência.", ErrorType.NotFound);

        return Result.Success(members);
    }
}

