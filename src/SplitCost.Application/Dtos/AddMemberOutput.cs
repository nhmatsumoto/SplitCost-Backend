﻿namespace SplitCost.Application.Dtos;

public record AddMemberOutput
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ResidenceId { get; set; }
}
