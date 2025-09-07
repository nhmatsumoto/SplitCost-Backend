namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakUserNotFoundException : KeycloakException
{
    public KeycloakUserNotFoundException(string message, object? details = null)
       : base(message, details) { }

    public KeycloakUserNotFoundException(string message)
        : base(message) { }

    public KeycloakUserNotFoundException(string message, string? details)
        : base(message, details) { }

    public KeycloakUserNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }

    public KeycloakUserNotFoundException(string message, string? details, Exception innerException)
        : base(message, details, innerException) { }
}
