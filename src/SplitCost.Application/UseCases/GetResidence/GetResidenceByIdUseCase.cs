using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;
using System.Threading;

namespace SplitCost.Application.UseCases.GetResidence;

public class GetResidenceByIdUseCase : IUseCase<GetResidenceByIdInput, Result<GetResidenceByIdOutput>>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IValidator<GetResidenceByIdInput> _validator;
    private readonly IMapper _mapper;

    public GetResidenceByIdUseCase(
        IResidenceRepository residenceRepository,
        IValidator<GetResidenceByIdInput> validator,
        IMapper mapper)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<GetResidenceByIdOutput>> ExecuteAsync(GetResidenceByIdInput userIdInput, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(userIdInput);

        if (!validationResult.IsValid)
        {
            return Result<GetResidenceByIdOutput>.FromFluentValidation("Validation failed", validationResult.Errors);
        }

        var residence = await _residenceRepository.GetByIdAsync(userIdInput.ResidenceId, cancellationToken);

        var output = _mapper.Map<GetResidenceByIdOutput>(residence);

        return Result<GetResidenceByIdOutput>.Success(output);
    }

    //public async Task<IEnumerable<ResidenceDto>> GetAllAsync()
    //{
    //    var residences = await _residenceRepository.GetAllAsync();

    //    return residences.Select(r => new ResidenceDto
    //    {
    //        Id = r.Id,
    //        Name = r.Name,
    //        CreatedAt = r.CreatedAt,
    //        UpdatedAt = r.UpdatedAt,

    //        Members = r.Members?.Select(m => new CreateResidenceMemberDto
    //        {
    //            UserId = m.UserId,
    //            ResidenceId = m.ResidenceId,
    //        })
    //        .ToList() ?? new List<CreateResidenceMemberDto>(),

    //        Expenses = r.Expenses?.Select(e => new ExpenseDto
    //        {
    //            Id = e.Id,
    //            Category = e.Category,
    //            Amount = e.Amount,
    //            Date = e.Date,
    //            IsSharedAmongMembers = e.IsSharedAmongMembers
    //        })
    //        .ToList() ?? new List<ExpenseDto>()

    //    });
    //}

    //public async Task<bool> UserHasResidence(Guid userId)
    //    => await _residenceRepository.UserHasResidence(userId);

}
