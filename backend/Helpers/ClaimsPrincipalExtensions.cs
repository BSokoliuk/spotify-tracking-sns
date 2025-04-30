
using System.Security.Claims;

namespace Helpers;

public static class ClaimsPrincipalExtensions
{
    public static string GetNameIdentifier(this ClaimsPrincipal user)
    {
        var nameIdentifier = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(nameIdentifier))
        {
            throw new UnauthorizedAccessException("User is not authenticated or token is invalid.");
        }
        return nameIdentifier;
    }

    public static List<string> GetRoles(this ClaimsPrincipal user)
    {
        return user.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
    }
}