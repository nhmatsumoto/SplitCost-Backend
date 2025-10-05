namespace SplitCost.Domain.Common;

public abstract class BaseEntity
{
    public Guid CreateBy { get; set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool Active { get; set; }
    public void UpdateTimestamps()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

