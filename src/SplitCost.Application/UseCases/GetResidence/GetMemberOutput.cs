namespace SplitCost.Application.UseCases.GetResidence;

public class GetMemberOutput
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ResidenceId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
