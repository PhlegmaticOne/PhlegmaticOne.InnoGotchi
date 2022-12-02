using System.Security.Claims;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;

namespace PhlegmaticOne.InnoGotchi.Web.Infrastructure.Helpers;

public class ClaimsPrincipalGenerator
{
    public static ClaimsPrincipal GenerateClaimsPrincipal(AuthorizedProfileDto authorizedProfileDto)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, authorizedProfileDto.Email),
            new(ProfileClaimsConstants.FirstNameClaimName, authorizedProfileDto.FirstName),
            new(ProfileClaimsConstants.SecondNameClaimName, authorizedProfileDto.LastName)
        };

        var claimsIdentity = new ClaimsIdentity(claims,
            Constants.CookieAuthenticationType,
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return new ClaimsPrincipal(claimsIdentity);
    }
}