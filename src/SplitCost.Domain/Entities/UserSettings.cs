namespace SplitCost.Domain.Entities;

public class UserSettings
{
    public Guid Id { get; set; }
    public bool ReceiveEmailNotifications { get; set; } = true;
    public string Theme { get; set; } = "Light";
    public string Language { get; set; } = "en";
    public Guid ResidenceId { get; set; }


    // Foreign Key
    public Guid UserId { get; set; }
    
    // Navigation Property
    public User User { get; set; } = null!;
}
