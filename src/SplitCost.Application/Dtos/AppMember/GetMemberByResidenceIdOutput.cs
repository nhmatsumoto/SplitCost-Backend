namespace SplitCost.Application.Dtos.AppMember;

public class GetMemberByresidenceIdOutput
{
    public Guid Id { get; set; }
    public string MemberName { get; set; }
    public Guid UserId { get; set; }
    public Guid ResidenceId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}
