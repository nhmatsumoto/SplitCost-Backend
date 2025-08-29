namespace SplitCost.Application.UseCases.Dtos;

public class MemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; }
    public string MemberName { get; set; } = string.Empty;
}
