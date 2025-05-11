using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Repository;

namespace SplitCost.Application.UseCases
{
    public class GetResidenceUseCase : IGetResidenceUseCase
    {
        private readonly IResidenceRepository _residenceRepository;

        public GetResidenceUseCase(IResidenceRepository residenceRepository)
        {
            _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        }

        public async Task<ResidenceDto?> GetByIdAsync(Guid id)
        {
            var residence = await _residenceRepository.GetByIdAsync(id);
            if (residence == null) return null;

            return new ResidenceDto
            {
                Id = residence.Id,
                Name = residence.Name,
                CreatedAt = residence.CreatedAt,
                UpdatedAt = residence.UpdatedAt
            };
        }

        public async Task<IEnumerable<ResidenceDto>> GetAllAsync()
        {
            var residences = await _residenceRepository.GetAllAsync();

            return residences.Select(r => new ResidenceDto
            {
                Id = r.Id,
                Name = r.Name,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });
        }
    }
}
