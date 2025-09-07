using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Application.Dtos;

namespace SplitCost.Application.UseCases
{
    public class UpdateResidenceUseCase : IUseCase<UpdateResidenceInput, Result<UpdateResidenceOutput>>
    {
        private readonly IResidenceRepository _residenceRepository;

        public UpdateResidenceUseCase(IResidenceRepository residenceRepository)
        {
            _residenceRepository = residenceRepository ?? throw new ArgumentNullException(nameof(residenceRepository));
        }

        public async Task<Result<UpdateResidenceOutput>> ExecuteAsync(UpdateResidenceInput updateResidenceInput, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //var residence = await _residenceRepository.GetByIdAsync(updateResidenceInput.ResidenceId, cancellationToken);
            //if (residence == null)
            //    throw new InvalidOperationException("Residência não encontrada.");

            //residence.SetName(updateResidenceInput.Name);
            //_residenceRepository.Update(residence);

            //return new ResidenceDto
            //{
            //    Id = residence.Id,
            //    Name = residence.Name,
            //    CreatedAt = residence.CreatedAt,
            //    UpdatedAt = residence.UpdatedAt
            //};

            //var result = new UpdateResidenceOutput
            //{
            //    Id = residence.Id,
            //    Name = residence.Name,
            //};

            var result = new UpdateResidenceOutput
            {
                Id = Guid.NewGuid(),
                Name = "NÃO IMPLEMENTADO",
            };

            return Result<UpdateResidenceOutput>.Success(result);
        }

       
    }
}
