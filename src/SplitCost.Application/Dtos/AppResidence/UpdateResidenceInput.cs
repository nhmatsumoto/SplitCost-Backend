namespace SplitCost.Application.Dtos.AppResidence;

public class UpdateResidenceInput
{
    public Guid ResidenceId { get; set; }
    public string Name { get; set; } = string.Empty;
}
