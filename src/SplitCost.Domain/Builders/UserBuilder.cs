using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Builders;

public class UserBuilder
{
    private readonly User _user = new();

    public UserBuilder WithId(Guid id) { _user.SetId(id); return this; }
    public UserBuilder WithUsername(string username) { _user.SetUsername(username); return this; }
    public UserBuilder WithName(string name) { _user.SetName(name); return this; }
    public UserBuilder WithEmail(string email) { _user.SetEmail(email); return this; }
    public UserBuilder WithAvatar(string avatarUrl) { _user.SetAvatarUrl(avatarUrl); return this; }

    public User Build() => _user;
}
