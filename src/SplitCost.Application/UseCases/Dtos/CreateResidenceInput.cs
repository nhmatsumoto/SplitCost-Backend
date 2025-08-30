namespace SplitCost.Application.UseCases.Dtos;

public record CreateResidenceInput
{
    public Guid UserId { get; set; }
    public string ResidenceName { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Apartment { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Prefecture { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}
