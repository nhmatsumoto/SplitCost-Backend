namespace SplitCost.Application.UseCases.CreateResidence;

public class CreateResidenceInput
{
    public Guid UserId { get; set; }
    public string ResidenceName { get; set; }
    public CreateAddressInput Address { get; set; }
}
