using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class UserFactory
{
    public static User Create() => new User();

    public static User Create(Guid id, string username, string name, string email, string avatarUrl = "")
    {
        return new User(id, username, name, email, avatarUrl);
    }
}
