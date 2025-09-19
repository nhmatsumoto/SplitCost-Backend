using FluentValidation;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.UseCases;

public class ReadResidenceByUserIdUseCase : IUseCase<GetResidenceByUserIdInput, Result<GetResidenceByUserIdOutput>>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IValidator<GetResidenceByUserIdInput> _validator;

    public ReadResidenceByUserIdUseCase(
        IResidenceRepository residenceRepository, 
        IValidator<GetResidenceByUserIdInput> validator)
    {
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _validator              = validator             ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<GetResidenceByUserIdOutput>> ExecuteAsync(GetResidenceByUserIdInput getResidenceByUserIdInput, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var validationResult = await _validator.ValidateAsync(getResidenceByUserIdInput);

        if (!validationResult.IsValid)
        {
            return Result<GetResidenceByUserIdOutput>.FromFluentValidation("Dados inválidos", validationResult.Errors);
        }

        //var residence = await _residenceRepository.GetByUserIdAsync(getResidenceByUserIdInput.UserId, cancellationToken);

        //var output = _mapper.Map<GetResidenceByUserIdOutput>(residence);

        var output = new GetResidenceByUserIdOutput();

        return Result<GetResidenceByUserIdOutput>.Success(output);
    }
}
