using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class CreateResidenceUseCase : ICreateResidenceUseCase
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IAddressRepository _addressRepository;

    public CreateResidenceUseCase(
        IResidenceRepository residenceRepository, 
        IAddressRepository addressRepository)
    {
        _residenceRepository = residenceRepository 
            ?? throw new ArgumentNullException(nameof(residenceRepository));

        _addressRepository = addressRepository 
            ?? throw new ArgumentNullException(nameof(addressRepository));
    }

    public async Task<ResidenceDto> CreateResidenceAsync(CreateResidenceDto createResidenceDto, Guid userId)
    {
        var address = new Address(
             createResidenceDto.Address.Street,
             createResidenceDto.Address.Number,
             createResidenceDto.Address.Apartment,
             createResidenceDto.Address.City,
             createResidenceDto.Address.Prefecture,
             createResidenceDto.Address.Country,
             createResidenceDto.Address.PostalCode
        );

        var residence = new Residence(createResidenceDto.ResidenceName, userId);

        residence.AddAdress(address);

        await _addressRepository.AddAsync(address);

        await _residenceRepository.AddAsync(residence);

        return new ResidenceDto
        {
            Id = residence.Id,
            Name = residence.Name,
            CreatedAt = residence.CreatedAt,
            UpdatedAt = residence.UpdatedAt,
            Address = new CreateAddressDto
            {
                Street = address.Street,
                Number = address.Number,
                Apartment = address.Apartment,
                City = address.City,
                Prefecture = address.Prefecture,
                Country = address.Country,
                PostalCode = address.PostalCode
            },
            Members = new List<ResidenceMemberDto>(),
            Expenses = new List<ResidenceExpenseDto>()
        };
    }
}
