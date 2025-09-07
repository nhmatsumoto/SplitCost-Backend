namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakForbiddenException : KeycloakException
{
    public KeycloakForbiddenException(string message, object? details = null)
        : base(message, details) { }
}
