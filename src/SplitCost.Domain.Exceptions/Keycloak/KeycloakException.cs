namespace SplitCost.Domain.Exceptions.Keycloak;

public abstract class KeycloakException : Exception
{
    protected KeycloakException(string message, object? details = null, Exception innerException = null)
        : base(message) { }
}
