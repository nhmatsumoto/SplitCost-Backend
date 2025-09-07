namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakUserConflictException : KeycloakException
{
    public KeycloakUserConflictException(string message, object? details = null)
        : base(message, details) { }
}
