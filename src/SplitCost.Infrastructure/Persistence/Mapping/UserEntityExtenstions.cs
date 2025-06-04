using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;
using SplitCost.Infrastructure.Persistence.Entities;

namespace SplitCost.Infrastructure.Persistence.Mapping;

public static class UserEntityExtenstions
{
    public static User ToDomain(this UserEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return UserFactory.Create(
            id: entity.Id,
            username: entity.Username,
            name: entity.Name,
            email: entity.Email,
            avatarUrl: entity.AvatarUrl ?? string.Empty
        );
    }

    public static UserEntity ToEntity(this User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        return new UserEntity
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl ?? string.Empty
        };
    }
}
