using FluentValidation;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.UseCases.IncomeUseCases.CreateIncome;

namespace SplitCost.Application.UseCases.Validations
{
    public class CreateIncomeInputValidation : AbstractValidator<CreateIncomeInput>
    {

        private readonly IIncomeRepository _incomeRepository;
        
        public CreateIncomeInputValidation(IIncomeRepository incomeRepository)
        {
            _incomeRepository = incomeRepository ?? throw new ArgumentNullException(nameof(incomeRepository));

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor da renda deve ser maior que zero.");
        }
    }
}
