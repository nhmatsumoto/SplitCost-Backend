namespace SplitCost.Application.Dtos;

public record AddMemberInput
{
    public Guid UserId { get; set; }
    public Guid ResidenceId { get; set; }
}
