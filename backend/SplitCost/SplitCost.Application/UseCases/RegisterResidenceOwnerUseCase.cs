using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases
{
    public class RegisterResidenceOwnerUseCase : IRegisterResidenceOwnerUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IResidenceRepository _residenceRepository;

        public RegisterResidenceOwnerUseCase(IUserRepository userRepository, IResidenceRepository residenceRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        }

        public async Task RegisterResidenceOwnerAsync(RegisterOwnerDto dto)
        {
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null)
                throw new InvalidOperationException("Usuário não encontrado.");

            var residence = await _residenceRepository.GetByIdAsync(dto.ResidenceId);
            if (residence == null)
                throw new InvalidOperationException("Residência não encontrada.");

            // Adicionar a associação do proprietário à residência
            //user.Id = residence.Id;

            residence.CreatedByUserId = user.Id;

            var ResidenceMember = new Member()
                .SetResidenceId(dto.ResidenceId)
                .SetUserId(dto.UserId)
                .SetJoinedAt(DateTime.UtcNow);

            residence.AddMember(ResidenceMember);

            await _userRepository.UpdateAsync(user);
        }
    }
}
