using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

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
                UpdatedAt = residence.UpdatedAt,

                Members = residence.Members.Select(x => new ResidenceMemberDto
                {
                    UserId = x.UserId,
                    UserName = x.User.Name
                })
                .ToList() ?? new List<ResidenceMemberDto>(),

                Expenses = residence.Expenses.Select(e => new ResidenceExpenseDto
                {
                    Id = e.Id,
                    ExpenseCategory = e.Category,
                    Amount = e.Amount,
                    Date = e.Date
                })
                .ToList() ?? new List<ResidenceExpenseDto>()
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
                UpdatedAt = r.UpdatedAt,

                Members = r.Members?.Select(m => new ResidenceMemberDto
                {
                    UserId = m.UserId,
                    UserName = m.User?.Name ?? string.Empty,
                })
                .ToList() ?? new List<ResidenceMemberDto>(),

                Expenses = r.Expenses?.Select(e => new ResidenceExpenseDto
                {
                    Id = e.Id,
                    ExpenseCategory = e.Category,
                    Amount = e.Amount,
                    Date = e.Date,
                    IsSharedAmongMembers = e.IsSharedAmongMembers
                })
                .ToList() ?? new List<ResidenceExpenseDto>()

            });
        }
    }
}
