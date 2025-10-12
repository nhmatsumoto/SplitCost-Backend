namespace SplitCost.Application.Dtos.Common;


/// <summary>
/// Input para resolver tenant no primeiro login
/// </summary>
public class ResolveTenantInput
{
    /// <summary>
    /// Nome da residência (somente se estiver criando uma nova)
    /// </summary>
    public string? ResidenceName { get; set; }

    /// <summary>
    /// Código de convite para entrar em uma residência existente
    /// </summary>
    public string? InviteCode { get; set; }

    // Dados opcionais do endereço, caso seja criação de residência
    public string? Street { get; set; }
    public string? Number { get; set; }
    public string? Apartment { get; set; }
    public string? City { get; set; }
    public string? Prefecture { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
}