using System.Security.Claims;

namespace Project.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        ArgumentNullException.ThrowIfNull(principal);

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null) throw new NullReferenceException("User ID claim not found.");
        if (!Guid.TryParse(userIdClaim.Value, out var userId)) throw new NullReferenceException("User ID claim is not a valid GUID.");

        return userId;
    }
}