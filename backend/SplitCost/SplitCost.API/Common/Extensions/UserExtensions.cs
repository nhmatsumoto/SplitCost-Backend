using System.Security.Claims;

namespace NextHome.API.Common.Extensions;

public static class UserExtensions
{
    public static string GetUserId(this ClaimsPrincipal user) 
        => user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    public static string GetUserEmail(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Email)?.Value;

    public static string GetUsername(this ClaimsPrincipal user)
        => user.FindFirst(ClaimTypes.Name)?.Value;

    public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
        => user.FindAll(ClaimTypes.Role).Select(c => c.Value);
}
