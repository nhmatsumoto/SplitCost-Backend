namespace SplitCost.Application.UseCases.Dtos;

public record CreateResidenceInput
{
    public Guid UserId { get; set; }
    public string ResidenceName { get; set; } = string.Empty;
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? Apartment { get; set; }
    public string? City { get; set; }
    public string? Prefecture { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}
