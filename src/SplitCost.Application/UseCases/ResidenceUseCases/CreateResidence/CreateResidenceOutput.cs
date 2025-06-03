using SplitCost.Application.UseCases.ExpenseUseCases;
using SplitCost.Application.UseCases.MemberUseCases.GetMember;

namespace SplitCost.Application.UseCases.ResidenceUseCases.CreateResidence;

public class CreateResidenceOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Apartment { get; private set; }
    public string City { get; private set; }
    public string Prefecture { get; private set; }
    public string Country { get; private set; }
    public string PostalCode { get; private set; }

    public List<GetMemberOutput> Members { get; set; } = new();
    public List<GetExpenseOutput> Expenses { get; set; } = new();

    // Auditoria
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

}
