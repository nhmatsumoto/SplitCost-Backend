using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadResidenceByIdUseCase : BaseUseCase<GetResidenceByIdInput, Residence>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IValidator<GetResidenceByIdInput> _validator;
    private readonly ILogger<ReadResidenceByIdUseCase> _logger;

    public ReadResidenceByIdUseCase(
        IResidenceRepository residenceRepository,
        IValidator<GetResidenceByIdInput> validator,
        ILogger<ReadResidenceByIdUseCase> logger)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(GetResidenceByIdInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<Residence>> HandleAsync(GetResidenceByIdInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando residência com Id {ResidenceId}", input.ResidenceId);

        var residence = await _residenceRepository.GetByIdAsync(input.ResidenceId, cancellationToken);

        if (residence == null)
        {
            _logger.LogWarning("Residência não encontrada: {ResidenceId}", input.ResidenceId);
            return Result<Residence>.Failure("Residence not found", ErrorType.NotFound);
        }

        return Result<Residence>.Success(residence);
    }
}
