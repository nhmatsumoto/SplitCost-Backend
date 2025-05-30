using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases
{
    public class CreateResidenceMember : ICreateResidenceMemberUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IResidenceRepository _residenceRepository;

        public CreateResidenceMember(IUserRepository userRepository, IResidenceRepository residenceRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        }

        public async Task RegisterResidenceMemberAsync(CreateResidenceMemberDto residenceMember)
        {
            var user = await _userRepository.GetByIdAsync(residenceMember.UserId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            var residence = await _residenceRepository.GetByIdAsync(residenceMember.ResidenceId);
            if (residence == null)
                throw new InvalidOperationException("Residência não encontrada.");

            residence.CreatedByUserId = user.Id;

            var member = new Member()
                .SetResidenceId(residenceMember.ResidenceId)
                .SetUserId(residenceMember.UserId)
                .SetJoinedAt(DateTime.UtcNow);

            residence.AddMember(member);

            _residenceRepository.UpdateAsync(residence);
        }
    }
}
