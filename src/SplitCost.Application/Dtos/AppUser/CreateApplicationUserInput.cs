namespace SplitCost.Application.Dtos.AppUser;

public record CreateApplicationUserInput
{
    public string Username { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;

    /// <summary>
    /// Define o perfil do usuário no sistema.
    /// Pode ser "Administrator" (responsável pela residência) ou "Member" (participante).
    /// </summary>
    public string Profile { get; init; } = string.Empty;
}
