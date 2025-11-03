using System.ComponentModel;

namespace SplitCost.Domain.Enums;

public enum UserProfileType
{
    [Description("Admin")]
    Administrator = 1,

    [Description("Member")]
    Member = 2
}



