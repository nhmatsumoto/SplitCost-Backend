namespace SplitCost.Application.UseCases.CreateMember;

public class AddResidenceMemberInput
{
    public Guid UserId { get; set; }
    public Guid ResidenceId { get; set; }
}
