using SplitCost.Domain.Enums;

namespace SplitCost.Domain.Extensions;

public static class ApplicationUserExtensions
{
    public static string GetUserProfileTypeDescription(UserProfileType profileType) => profileType switch
    {
        UserProfileType.Administrator => "Administrator",
        UserProfileType.Member => "Member",
        _ => "Unknown"
    };
}
