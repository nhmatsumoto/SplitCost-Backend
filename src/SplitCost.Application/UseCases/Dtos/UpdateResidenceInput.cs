namespace SplitCost.Application.UseCases.Dtos;

public class UpdateResidenceInput
{
    public Guid ResidenceId { get; set; }
    public string Name { get; set; } = string.Empty;
}
