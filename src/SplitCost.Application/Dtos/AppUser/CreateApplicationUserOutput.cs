using SplitCost.Domain.Enums;

namespace SplitCost.Application.Dtos.AppUser;

public record CreateApplicationUserOutput
{
    /// <summary>
    /// Identificador único do usuário
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Nome de usuário escolhido
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Primeiro nome do usuário
    /// </summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>
    /// Sobrenome do usuário
    /// </summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>
    /// E-mail do usuário
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Perfil/Role do usuário no sistema
    /// </summary>
    public UserProfileType Profile { get; init; }

    /// <summary>
    /// Data de criação do usuário
    /// </summary>
    public DateTime CreatedAt { get; init; }
}
