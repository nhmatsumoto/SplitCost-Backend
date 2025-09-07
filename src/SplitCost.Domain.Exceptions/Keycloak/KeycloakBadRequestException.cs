namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakBadRequestException : KeycloakException
{
    public KeycloakBadRequestException(string message, object? details = null)
        : base(message, details) { }
}
