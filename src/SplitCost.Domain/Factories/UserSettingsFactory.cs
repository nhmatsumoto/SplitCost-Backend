using SplitCost.Domain.Entities;

namespace SplitCost.Domain.Factories;

public static class UserSettingsFactory
{
    public static UserSettings Create() => new UserSettings();

    public static UserSettings Create(
        Guid userSettingsId,
        bool receiveEmailNotifications,
        string theme,
        string language,
        Guid userId,
        Guid residenceId
        ) =>
            new UserSettings()
                .SetId(userSettingsId)
                .SetReceiveEmailNotifications(receiveEmailNotifications)
                .SetTheme(theme)
                .SetLanguage(language)
                .SetUserId(userId)
                .SetResidenceId(residenceId);
        


}
