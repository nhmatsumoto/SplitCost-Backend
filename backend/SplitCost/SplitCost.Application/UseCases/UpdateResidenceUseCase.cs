using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Repository;

namespace SplitCost.Application.UseCases
{
    public class UpdateResidenceUseCase : IUpdateResidenceUseCase
    {
        private readonly IResidenceRepository _residenceRepository;

        public UpdateResidenceUseCase(IResidenceRepository residenceRepository)
        {
            _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        }

        public async Task<ResidenceDto> UpdateResidenceAsync(Guid residenceId, string name)
        {
            var residence = await _residenceRepository.GetByIdAsync(residenceId);
            if (residence == null)
                throw new InvalidOperationException("Residência não encontrada.");

            residence.Name = name;
            await _residenceRepository.UpdateAsync(residence);

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
