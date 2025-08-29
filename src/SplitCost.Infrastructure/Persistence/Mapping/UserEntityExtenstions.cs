using SplitCost.Domain.Entities;
using SplitCost.Domain.Factories;

namespace SplitCost.Infrastructure.Persistence.Mapping;

public static class UserEntityExtenstions
{
    public static User ToDomain(this User entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return UserFactory
            .Create()
            .SetId(entity.Id)
            .SetUsername(entity.Username)
            .SetName(entity.Name)
            .SetEmail(entity.Email)
            .SetAvatarUrl(entity.AvatarUrl ?? string.Empty);
    }

    public static User ToEntity(this User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        return new User
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Name,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl ?? string.Empty
        };
    }
}
