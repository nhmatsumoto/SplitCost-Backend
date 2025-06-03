using SplitCost.Application.UseCases.ExpenseUseCases;
using SplitCost.Application.UseCases.MemberUseCases.GetMember;

namespace SplitCost.Application.UseCases.ResidenceUseCases.GetResidenceById;

public class GetResidenceByIdOutput
{
    public Guid Id { get;  set; }
    public Guid CreatedByUserId { get; set; }
    public string Name { get;  set; }
    public string Street { get;  set; }
    public string Number { get;  set; }
    public string Apartment { get; set; }
    public string City { get;  set; }
    public string Prefecture { get; set; }
    public string Country { get; set; }
    public string PostalCode { get; set; }
    public List<GetMemberOutput> Members { get; set; }
    public List<GetExpenseOutput> Expenses { get; set; }
}
