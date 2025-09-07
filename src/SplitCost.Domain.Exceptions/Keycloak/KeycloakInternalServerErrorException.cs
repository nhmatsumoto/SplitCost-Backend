namespace SplitCost.Domain.Exceptions.Keycloak;

public class KeycloakInternalServerErrorException : KeycloakException
{
    public KeycloakInternalServerErrorException(string message, object? details = null)
        : base(message, details) { }
}