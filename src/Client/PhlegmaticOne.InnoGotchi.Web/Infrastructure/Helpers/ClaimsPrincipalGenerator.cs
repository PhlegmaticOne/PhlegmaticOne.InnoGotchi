using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using System.Security.Claims;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;

public class ClaimsPrincipalGenerator
{
    public static ClaimsPrincipal GenerateClaimsPrincipal(AuthorizedProfileDto authorizedProfileDto)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, authorizedProfileDto.Email),
            new(ProfileClaimsConstants.FirstNameClaimName, authorizedProfileDto.FirstName),
            new(ProfileClaimsConstants.LastNameClaimName, authorizedProfileDto.LastName)
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            Constants.CookieAuthenticationType,
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return new ClaimsPrincipal(claimsIdentity);
    }
}