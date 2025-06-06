﻿using MapsterMapper;
using SplitCost.Application.Common;
using SplitCost.Application.Common.Interfaces;
using SplitCost.Application.Common.Repositories;
using SplitCost.Application.Common.Responses;

namespace SplitCost.Application.UseCases.ApplicationUserUseCases.GetApplicationUserById;

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
            return Result<GetApplicationUserByIdOutput>.Failure(Messages.UserNotFound, ErrorType.NotFound);
        }

        var user = _mapper.Map<GetApplicationUserByIdOutput>(result);

        return Result<GetApplicationUserByIdOutput>.Success(user);
    }

}
