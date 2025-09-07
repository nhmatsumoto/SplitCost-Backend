namespace SplitCost.Application.Dtos;

public class GetResidenceByUserIdOutput
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

    // Auditoria
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Coleções
    public List<AddMemberInput> Members { get; set; } = new List<AddMemberInput>();
    public List<GetExpenseOutput> Expenses { get; set; } = new List<GetExpenseOutput>();


    public GetResidenceByUserIdOutput()
    {
        
    }
}