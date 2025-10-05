using SplitCost.Domain.Common;

namespace SplitCost.Domain.Entities;

public class UserSettings : BaseEntity
{
    public Guid Id { get; private set; }
    public bool ReceiveEmailNotifications { get; private set; } = true;
    public string? Theme { get; private set; }
    public string? Language { get; private set; }
    
    // Foreign Key
    public Guid UserId { get; private set; }
    public Guid? ResidenceId { get; private set; }

    public Residence Residence { get; private set; } = null!;


    internal UserSettings()
    {
        
    }

    public UserSettings SetId(Guid userSettingsId) 
    {
    
        if (userSettingsId == Guid.Empty) throw new ArgumentException("Id inválido.");
        Id = userSettingsId;
        return this;
    }

    public UserSettings SetReceiveEmailNotifications(bool receiveEmailNotifications)
    {
        ReceiveEmailNotifications = receiveEmailNotifications;
        return this;
    }

    public UserSettings SetTheme(string theme)
    {
        if (string.IsNullOrWhiteSpace(theme)) throw new ArgumentException("Tema inválido.");
        Theme = theme;
        return this;
    }

    public UserSettings SetLanguage(string language)
    {
        if (string.IsNullOrWhiteSpace(language)) throw new ArgumentException("Idioma inválido.");
        Language = language;
        return this;
    }

    public UserSettings SetUserId(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId inválido.");
        UserId = userId;
        return this;
    }

    public UserSettings SetResidenceId(Guid residenceId)
    {
        if (residenceId == Guid.Empty) throw new ArgumentException("ResidenceId inválido.");
        ResidenceId = residenceId;
        return this;
    }
}
