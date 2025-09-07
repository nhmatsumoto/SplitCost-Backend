using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.Validations
{
    public class CreateIncomeInputValidation : AbstractValidator<CreateIncomeInput>
    {

   
        private readonly IResidenceRepository _residenceRepository;
        private readonly IUserRepository _userRepository;
        
        public CreateIncomeInputValidation( 
            IResidenceRepository residenceRepository, 
            IUserRepository userRepository)
        {
           
            _residenceRepository    = residenceRepository   ?? throw new ArgumentNullException(nameof(residenceRepository));
            _userRepository         = userRepository        ?? throw new ArgumentNullException(nameof(userRepository));

            // Validação do Amount
            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("O valor da renda deve ser maior que zero.");

            // Validação do Category
            RuleFor(x => x.Category)
                .IsInEnum()
                .WithMessage("A categoria de renda é inválida.");

            // Validação do Date
            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("A data da renda não pode ser vazia.");

            // Validação do Description
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A descrição não pode ser vazia.")
                .MaximumLength(500)
                .WithMessage("A descrição não pode exceder 500 caracteres.");

            // Validação do ResidenceId
            RuleFor(x => x.ResidenceId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID da residência é inválido.")
                .MustAsync(CheckIfResidenceExists)
                .WithMessage("A residência informada não existe.");

            // Validação do RegisteredByUserId
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("O ID do usuário é inválido.")
                .MustAsync(CheckIfUserExists)
                .WithMessage("O usuário informado não existe.");
        }

        private async Task<bool> CheckIfResidenceExists(Guid residenceId, CancellationToken ct)
        {
            return await _residenceRepository.ExistsAsync(x => x.Id == residenceId, ct);
        }

        private async Task<bool> CheckIfUserExists(Guid userId, CancellationToken ct)
        {
            return await _userRepository.ExistsAsync(x => x.Id == userId, ct);
        }
    }
}
