
using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Factories;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases;

public class CreateResidenceUseCase : ICreateResidenceUseCase
{
    private readonly IResidenceRepository _residenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public CreateResidenceUseCase(
        IResidenceRepository residenceRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
        _unitOfWork             = unitOfWork            ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper                 = mapper                ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<ResidenceDto>> CreateResidenceAsync(CreateResidenceDto dto, Guid userId)
    {
        var address = AddressFactory.Create(
            dto.Address.Street,
            dto.Address.Number, 
            dto.Address.Apartment, 
            dto.Address.City,
            dto.Address.Prefecture, 
            dto.Address.Country, 
            dto.Address.PostalCode);

        if(address == null)
        {
            return Result<ResidenceDto>.Failure("Endereço inválido.", ErrorType.Validation);
        }

        var residence = ResidenceFactory.Create(dto.ResidenceName, userId)
            .SetAddress(address);

        if (residence == null)
        {
            return Result<ResidenceDto>.Failure("Residência inválida.", ErrorType.Validation);
        }

        //try
        //{
        //    await _residenceRepository.AddAsync(residence);
        //    await _unitOfWork.SaveChangesAsync();

        //    var residenceDto = _mapper.Map<ResidenceDto>(residence);

        //    return Result.Success(dto);
        //}
        //catch (Exception ex)
        //{
        //    return Result.Failure($"Erro ao criar residência: {ex.Message}", ErrorType.InternalError);
        //}

        await _residenceRepository.AddAsync(residence);
        await _unitOfWork.SaveChangesAsync();

        return Result<ResidenceDto>.Success(_mapper.Map<ResidenceDto>(residence));
    }
}
