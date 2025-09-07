namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakUserCreationFailedException : KeycloakException
{
    public KeycloakUserCreationFailedException(string message, object? details = null)
        : base(message, details) { }
}
