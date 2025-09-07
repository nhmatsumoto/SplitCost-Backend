namespace SplitCost.Application.Dtos;

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
    public List<GetMemberByresidenceIdOutput> Members { get; set; }
    public List<GetExpenseOutput> Expenses { get; set; }
}
