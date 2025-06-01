using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases.GetResidence;

public class GetResidenceByUserIdUseCase : IUseCase<GetResidenceByUserIdInput, Result<GetResidenceByUserIdOutput>>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<GetResidenceByUserIdInput> _validator;

    public GetResidenceByUserIdUseCase(
        IResidenceRepository residenceRepository, 
        IMapper mapper,
        IValidator<GetResidenceByUserIdInput> validator)
    {
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _mapper                 = mapper                ?? throw new ArgumentNullException(nameof(mapper));
        _validator              = validator             ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<GetResidenceByUserIdOutput>> ExecuteAsync(GetResidenceByUserIdInput getResidenceByUserIdInput)
    {
        var validationResult = await _validator.ValidateAsync(getResidenceByUserIdInput);

        if (!validationResult.IsValid)
        {
            return Result<GetResidenceByUserIdOutput>.FromFluentValidation("Dados inválidos", validationResult.Errors);
        }

        var residence = await _residenceRepository.GetByUserIdAsync(getResidenceByUserIdInput.UserId);
        
        var output = _mapper.Map<GetResidenceByUserIdOutput>(residence);

        return Result<GetResidenceByUserIdOutput>.Success(output);
    }
}
