using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class ReadResidenceByIdUseCase : IUseCase<GetResidenceByIdInput, Result<Residence>>
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IValidator<GetResidenceByIdInput> _validator;

    public ReadResidenceByIdUseCase(
        IResidenceRepository residenceRepository,
        IValidator<GetResidenceByIdInput> validator)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Result<Residence>> ExecuteAsync(GetResidenceByIdInput userIdInput, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var validationResult = await _validator.ValidateAsync(userIdInput);

        if (!validationResult.IsValid)
        {
            return Result<Residence>.FromFluentValidation("Validation failed", validationResult.Errors);
        }

        var residence = await _residenceRepository.GetByIdAsync(userIdInput.ResidenceId, cancellationToken);

        return Result<Residence>.Success(residence);
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
