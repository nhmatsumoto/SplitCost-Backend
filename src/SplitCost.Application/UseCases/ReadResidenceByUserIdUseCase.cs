using FluentValidation;
using Microsoft.Extensions.Logging;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Common.UseCases;
using SplitCost.Application.Dtos.AppResidence;

namespace SplitCost.Application.UseCases;

public class ReadResidenceByUserIdUseCase : BaseUseCase<GetResidenceByUserIdInput, GetResidenceByUserIdOutput>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IValidator<GetResidenceByUserIdInput> _validator;
    private readonly ILogger<ReadResidenceByUserIdUseCase> _logger;

    public ReadResidenceByUserIdUseCase(
        IResidenceRepository residenceRepository,
        IValidator<GetResidenceByUserIdInput> validator,
        ILogger<ReadResidenceByUserIdUseCase> logger)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<FluentValidation.Results.ValidationResult> ValidateAsync(GetResidenceByUserIdInput input, CancellationToken cancellationToken)
    {
        if (input == null)
        {
            return new FluentValidation.Results.ValidationResult(
                new[] { new FluentValidation.Results.ValidationFailure(nameof(input), "Input não pode ser nulo") }
            );
        }

        return await _validator.ValidateAsync(input, cancellationToken);
    }

    protected override async Task<Result<GetResidenceByUserIdOutput>> HandleAsync(GetResidenceByUserIdInput input, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        _logger.BeginScope("Buscando residência para usuário {UserId}", input.UserId);

        var residence = await _residenceRepository.GetByUserIdAsync(input.UserId, cancellationToken);

        if (residence == null)
        {
            _logger.LogWarning("Residência não encontrada para usuário {UserId}", input.UserId);
            return Result<GetResidenceByUserIdOutput>.Failure("Residence not found", ErrorType.NotFound);
        }

        var output = Mapper.Map<Domain.Entities.Residence, GetResidenceByUserIdOutput>(residence);

        return Result<GetResidenceByUserIdOutput>.Success(output);
    }
}
