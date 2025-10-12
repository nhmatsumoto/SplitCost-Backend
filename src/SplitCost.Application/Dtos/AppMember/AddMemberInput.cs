namespace SplitCost.Application.Dtos.AppMember;

public record AddMemberInput
{
    public Guid UserId { get; set; }
    public Guid ResidenceId { get; set; }
}
