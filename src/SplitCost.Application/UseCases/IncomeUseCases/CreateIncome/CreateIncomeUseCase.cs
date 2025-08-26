using FluentValidation;
using MapsterMapper;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases.IncomeUseCases.CreateIncome
{
    public class CreateIncomeUseCase : IUseCase<CreateIncomeInput, Result<CreateIncomeOutput>>
    {
        private readonly IIncomeRepository _incomeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateIncomeInput> _validator;

        public CreateIncomeUseCase(IIncomeRepository incomeRepository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<CreateIncomeInput> validator)
        {
            _incomeRepository = incomeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<Result<CreateIncomeOutput>> ExecuteAsync(CreateIncomeInput input, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(input, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Result<CreateIncomeOutput>.FromFluentValidation("Dados inválidos", validationResult.Errors);
            }

            var income = _mapper.Map<Income>(input);

            await _incomeRepository.AddAsync(income, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            var result = _mapper.Map<CreateIncomeOutput>(income);

            return Result<CreateIncomeOutput>.Success(result);
        }
    }
}
