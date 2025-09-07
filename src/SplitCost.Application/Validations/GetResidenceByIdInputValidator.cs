using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.Validations;


public class GetResidenceByIdInputValidator : AbstractValidator<GetResidenceByIdInput>
{

    private readonly IResidenceRepository _residenceRepository;
    public GetResidenceByIdInputValidator(IResidenceRepository residenceRepository)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));

        RuleFor(x => x.ResidenceId)
           .NotEmpty()
           .WithMessage("Residence is required.")
           .MustAsync(ResidenceExists)
           .WithMessage("Residence not found.");
    }

    private async Task<bool> ResidenceExists(Guid userId, CancellationToken ct)
    {
        return await _residenceRepository.ExistsAsync(x => x.Id == userId, ct);
    }
}
