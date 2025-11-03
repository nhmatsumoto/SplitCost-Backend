namespace SplitCost.Infrastructure.Services.Auth;

public class Credential
{
    public string type { get; set; } = "password";
    public string value { get; set; } = string.Empty;
    public bool temporary { get; set; } = false;
}
