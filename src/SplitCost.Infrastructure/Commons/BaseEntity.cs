﻿namespace SplitCost.Infrastructure.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }

    public void UpdateTimestamps()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}

