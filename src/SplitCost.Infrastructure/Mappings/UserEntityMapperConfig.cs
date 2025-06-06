﻿using Mapster;
using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Mappers;

public class UserEntityMapperConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Domain -> Entity
        config.NewConfig<User, UserEntity>()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Name, src => src.Name)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Id, src => src.Id);

        // Entity -> Domain
        config.NewConfig<UserEntity, User>()
            .MapWith(src => UserFactory.Create(
                src.Id,
                src.Username,
                src.Name,
                src.Email,
                src.AvatarUrl));
    }
}
