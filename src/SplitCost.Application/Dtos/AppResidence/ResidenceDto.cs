using SplitCost.Application.Dtos.AppExpense;
using SplitCost.Application.Dtos.AppMember;

namespace SplitCost.Application.Dtos.AppResidence;

public class ResidenceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Apartment { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Prefecture { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public Guid CreatedByUserId { get; set; }

    public List<MemberDto> Members { get; set; } = new();
    public List<ExpenseDto> Expenses { get; set; } = new();
}

