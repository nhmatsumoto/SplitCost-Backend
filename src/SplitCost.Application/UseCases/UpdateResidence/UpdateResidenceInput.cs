using SplitCost.Application.DTOs;

namespace SplitCost.Application.UseCases.UpdateResidence;

public class UpdateResidenceInput
{
    public Guid ResidenceId { get; set; }
    public string Name { get; set; }
    public List<CreateResidenceMemberDto> Members { get; set; }
    public List<CreateExpenseDto> Expenses { get; set; }
}
