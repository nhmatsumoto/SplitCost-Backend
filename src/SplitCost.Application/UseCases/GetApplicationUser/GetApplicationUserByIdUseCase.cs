using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Interfaces;
using SplitCost.Domain.Interfaces;

namespace SplitCost.Application.UseCases.GetApplicationUser;

public class GetApplicationUserByIdUseCase : IUseCase<Guid, Result<GetApplicationUserByIdOutput>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetApplicationUserByIdUseCase(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository     = userRepository    ?? throw new ArgumentException(nameof(userRepository));
        _mapper             = mapper            ?? throw new ArgumentException(nameof(mapper));
    }

    public async Task<Result<GetApplicationUserByIdOutput>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (result == null)
        {
            return Result<GetApplicationUserByIdOutput>.Failure($"Expense not found.", ErrorType.NotFound);
        }

        var user = _mapper.Map<GetApplicationUserByIdOutput>(result);

        return Result<GetApplicationUserByIdOutput>.Success(user);
    }

}
