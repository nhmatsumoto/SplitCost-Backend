using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Repository;

namespace SplitCost.Application.UseCases;

public class CreateResidenceUseCase : ICreateResidenceUseCase
{
    private readonly IResidenceRepository _residenceRepository;

    public CreateResidenceUseCase(IResidenceRepository residenceRepository)
    {
        _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
    }

    public async Task<ResidenceDto> CreateResidenceAsync(string name)
    {
        var residence = new Residence(name);
        await _residenceRepository.AddAsync(residence);

        return new ResidenceDto
        {
            Id = residence.Id,
            Name = residence.Name,
            CreatedAt = residence.CreatedAt,
            UpdatedAt = residence.UpdatedAt,
            Members = new List<ResidenceMemberDto>(),
            Expenses = new List<ExpenseDto>()
        };
    }
}
