using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;
using System.Security.Policy;

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

    public async Task<bool> CreateEmptyResidence(Guid userId)
    {
        try
        {
            var residence = new Residence()
           .SetCreatedByUser(userId);

            await _residenceRepository.AddAsync(residence);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<ResidenceDto> CreateResidenceAsync(CreateResidenceDto dto, Guid userId)
    {
        //Map CreateResidenceDto to Residence
        //Map CreateAddressDto to Address

        var address = new Address()
            .SetStreet(dto.Address.Street)
            .SetNumber(dto.Address.Number)
            .SetApartment(dto.Address.Apartment)
            .SetCity(dto.Address.City)
            .SetPrefecture(dto.Address.Prefecture)
            .SetCountry(dto.Address.Country)
            .SetPostalCode(dto.Address.PostalCode);

        var residence = new Residence()
            .SetName(dto.ResidenceName)
            .SetCreatedByUser(userId)
            .SetAddress(address);

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
            Expenses = new List<ExpenseDto>()
        };
    }
}
