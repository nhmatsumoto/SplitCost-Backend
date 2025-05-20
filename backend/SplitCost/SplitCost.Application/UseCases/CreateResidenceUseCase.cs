using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class CreateResidenceUseCase : ICreateResidenceUseCase
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateResidenceUseCase(
        IResidenceRepository residenceRepository,
        IUnitOfWork unitOfWork)
    {
        _residenceRepository = residenceRepository 
            ?? throw new ArgumentNullException(nameof(residenceRepository));

        _unitOfWork = unitOfWork
            ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ResidenceDto> CreateResidenceAsync(CreateResidenceDto createResidenceDto, Guid userId)
    {
        var residence = new Residence(createResidenceDto.ResidenceName, userId);

        residence.SetAddress(
            createResidenceDto.Address.Street,
            createResidenceDto.Address.Number,
            createResidenceDto.Address.Apartment,
            createResidenceDto.Address.City,
            createResidenceDto.Address.Prefecture,
            createResidenceDto.Address.Country,
            createResidenceDto.Address.PostalCode
        );

        await _residenceRepository.AddAsync(residence);

        await _unitOfWork.SaveChangesAsync();

        return new ResidenceDto
        {
            Id = residence.Id,
            Name = residence.Name,
            CreatedAt = residence.CreatedAt,
            UpdatedAt = residence.UpdatedAt,
            Address = new AddressDto 
            {
                Street = residence.Address.Street,
                Number = residence.Address.Number,
                Apartment = residence.Address.Apartment,
                City = residence.Address.City,
                Prefecture = residence.Address.Prefecture,
                Country = residence.Address.Country,
                PostalCode = residence.Address.PostalCode
            },
            Members = new List<ResidenceMemberDto>(),
            Expenses = new List<CreateExpenseDto>()
        };
    }
}
