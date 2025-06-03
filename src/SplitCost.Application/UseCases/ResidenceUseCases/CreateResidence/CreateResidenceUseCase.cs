
using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases.ResidenceUseCases.CreateResidence;

public class CreateResidenceUseCase : IUseCase<CreateResidenceInput, Result<CreateResidenceOutput>>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateResidenceInput> _validator;
    
    public CreateResidenceUseCase(
        IResidenceRepository residenceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateResidenceInput> validator)
    {
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper                 = mapper                ?? throw new ArgumentNullException(nameof(mapper));
        _validator              = validator             ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<CreateResidenceOutput>> ExecuteAsync(CreateResidenceInput input, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(input);

        if (!validationResult.IsValid)
        {
            return Result<CreateResidenceOutput>.FromFluentValidation(Messages.InvalidData, validationResult.Errors);
        }

        var residence = ResidenceFactory.Create(
            input.ResidenceName,
            input.UserId,
            input.Street,
            input.Number,
            input.Apartment,
            input.City,
            input.Prefecture,
            input.Country,
            input.PostalCode
        );

        await _residenceRepository.AddAsync(residence, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var resultResidence = await _residenceRepository.GetByUserIdAsync(input.UserId, cancellationToken);

        return Result<CreateResidenceOutput>.Success(_mapper.Map<CreateResidenceOutput>(resultResidence));
    }
}
