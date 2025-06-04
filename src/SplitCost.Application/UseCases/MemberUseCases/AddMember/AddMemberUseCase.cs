using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Application.UseCases.MemberUseCases.AddMember;

public class AddMemberUseCase : IUseCase<AddMemberInput, Result<int>>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IValidator<AddMemberInput> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddMemberUseCase(
        IUserRepository userRepository, 
        IResidenceRepository residenceRepository, 
        IValidator<AddMemberInput> validator,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<int>> ExecuteAsync(AddMemberInput residenceMemberInput, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(residenceMemberInput);

        if(!validationResult.IsValid)
        {
            return Result<int>.FromFluentValidation("Dados inválidos", validationResult.Errors);
        }

        var residence = _mapper.Map<Residence>(residenceMemberInput);

        residence.SetCreatedByUser(residenceMemberInput.UserId);

        //residenceMemberInput.ResidenceId, residenceMemberInput.UserId, 
        var member = MemberFactory.Create(DateTime.UtcNow);

        residence.AddMember(member);

        await _residenceRepository.AddAsync(residence, cancellationToken);

        //var residenceEntity = await _residenceRepository.GetByIdAsync(residenceMemberInput.ResidenceId);
#warning refatorar

        return Result<int>.Success(1);
    }
}