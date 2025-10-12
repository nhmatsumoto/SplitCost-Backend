namespace SplitCost.Application.Dtos.AppMember;

public class MemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string MemberName { get; set; } = string.Empty;
}
