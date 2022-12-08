using PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;
using System.Security.Claims;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string Firstname(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.FirstNameClaimName);
        return claimValue is null ? string.Empty : claimValue.Value;
    }

    public static string Lastname(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims
            .FirstOrDefault(x => x.Type == ProfileClaimsConstants.LastNameClaimName);
        return claimValue is null ? string.Empty : claimValue.Value;
    }
}