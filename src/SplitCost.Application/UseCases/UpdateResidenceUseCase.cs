using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases
{
    public class UpdateResidenceUseCase : IUpdateResidenceUseCase
    {
        private readonly IResidenceRepository _residenceRepository;

        public UpdateResidenceUseCase(IResidenceRepository residenceRepository)
        {
            _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        }

        public async Task<ResidenceDto> UpdateResidenceAsync(Guid residenceId, UpdateResidenceDto residenceDto)
        {
            var residence = await _residenceRepository.GetByIdAsync(residenceId);
            if (residence == null)
                throw new InvalidOperationException("Residência não encontrada.");

            residence.SetName(residenceDto.Name);
            _residenceRepository.UpdateAsync(residence);

            return new ResidenceDto
            {
                Id = residence.Id,
                Name = residence.Name,
                CreatedAt = residence.CreatedAt,
                UpdatedAt = residence.UpdatedAt
            };
        }
    }
}
