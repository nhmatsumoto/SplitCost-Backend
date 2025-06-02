using SplitCost.Domain.Entities;

namespace SplitCost.Application.UseCases.GetResidence;

public class GetResidenceByIdOutput
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid AddressId { get; private set; }
    public Guid CreatedByUserId { get; private set; }
    public List<GetMemberOutput> Members { get; set; }
    public List<GetExpenseOutput> Expenses { get; set; }
}
