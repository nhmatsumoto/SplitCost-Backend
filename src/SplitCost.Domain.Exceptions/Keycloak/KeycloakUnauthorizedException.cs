namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakUnauthorizedException : KeycloakException
{
    public KeycloakUnauthorizedException(string message, object? details = null)
        : base(message, details) { }
}
