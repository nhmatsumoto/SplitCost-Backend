using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;
using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases;

public class GetApplicationUserByIdUseCase : IUseCase<Guid, Result<User>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetApplicationUserByIdUseCase(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository     = userRepository    ?? throw new ArgumentException(nameof(userRepository));
        _mapper             = mapper            ?? throw new ArgumentException(nameof(mapper));
    }

    public async Task<Result<User>> ExecuteAsync(Guid id, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (result == null)
        {
            return Result<User>.Failure(Messages.UserNotFound, ErrorType.NotFound);
        }

        return Result<User>.Success(result);
    }

}
