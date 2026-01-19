using System.Security.Claims;

namespace TrustGateAPI.Security;

public static class ClaimsPrincipalExtensions
{
    public static int GetCompanyId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("companyId");
        if (claim == null)
            throw new UnauthorizedAccessException("Company context required");

        return int.Parse(claim.Value);
    }

    public static int? GetCompanyIdOrNull(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("companyId");
        return claim == null ? null : int.Parse(claim.Value);
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        return user.IsInRole("Admin");
    }
}
