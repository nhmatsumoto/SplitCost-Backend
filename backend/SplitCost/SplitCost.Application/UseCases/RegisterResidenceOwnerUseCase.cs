using SplitCost.Application.DTOs;
using SplitCost.Application.Interfaces;
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

            user.Id = residence.Id;  // Adicionar a associação do proprietário à residência
            await _userRepository.UpdateAsync(user);
        }
    }
}
