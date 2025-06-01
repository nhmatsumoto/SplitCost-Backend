using SplitCost.Application.DTOs;

namespace SplitCost.Application.UseCases.CreateResidence;

public class CreateResidenceOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<CreateResidenceMemberDto> Members { get; set; }
    public List<ExpenseDto> Expenses { get; set; }
    public AddressDto Address { get; set; }

    public CreateResidenceOutput()
    {
        Members = new List<CreateResidenceMemberDto>();
        Expenses = new List<ExpenseDto>();
    }
}
