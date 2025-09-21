using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.UseCases;

public class UpdateResidenceUseCase : BaseUseCase<UpdateResidenceInput, UpdateResidenceOutput>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly ILogger<UpdateResidenceUseCase> _logger;

    public UpdateResidenceUseCase(
        IResidenceRepository residenceRepository,
        ILogger<UpdateResidenceUseCase> logger)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override Task<FluentValidation.Results.ValidationResult> ValidateAsync(UpdateResidenceInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return Task.FromResult(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            ));
        }

        if (input.ResidenceId == Guid.Empty)
        {
            return Task.FromResult(new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input.ResidenceId), "ResidenceId não pode ser vazio") }
            ));
        }

        return Task.FromResult(new FluentValidation.Results.ValidationResult());
    }

    protected override async Task<Result<UpdateResidenceOutput>> HandleAsync(UpdateResidenceInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Atualizando residência {ResidenceId}", input.ResidenceId);

        var residence = await _residenceRepository.GetByIdAsync(input.ResidenceId, cancellationToken);

        if (residence == null)
        {
            _logger.LogWarning("Residência não encontrada: {ResidenceId}", input.ResidenceId);
            return Result<UpdateResidenceOutput>.Failure("Residence not found", ErrorType.NotFound);
        }

        residence.SetName(input.Name);
        await _residenceRepository.UpdateAsync(residence);

        var output = new UpdateResidenceOutput
        {
            Id = residence.Id,
            Name = residence.Name
        };

        return Result<UpdateResidenceOutput>.Success(output);
    }
}
