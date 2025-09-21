using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Builders;

public class UserSettingsBuilder
{
    private readonly UserSettings _settings = new();

    public UserSettingsBuilder WithUserId(Guid userId) { _settings.SetUserId(userId); return this; }
    public UserSettingsBuilder WithTheme(string theme) { _settings.SetTheme(theme); return this; }
    public UserSettingsBuilder WithLanguage(string language) { _settings.SetLanguage(language); return this; }

    public UserSettings Build() => _settings;
}
