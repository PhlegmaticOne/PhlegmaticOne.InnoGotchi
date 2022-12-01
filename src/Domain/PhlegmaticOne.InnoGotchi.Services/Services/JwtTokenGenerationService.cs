using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.JwtToken;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.JwtTokensGeneration;
using PhlegmaticOne.JwtTokensGeneration.Models;

namespace PhlegmaticOne.InnoGotchi.Services.Services;

public class JwtTokenGenerationService : IJwtTokenGenerationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public JwtTokenGenerationService(IJwtTokenGenerator jwtTokenGenerator) =>
        _jwtTokenGenerator = jwtTokenGenerator;

    public JwtTokenDto GenerateJwtToken(AuthorizedProfileDto profile)
    {
        var userInfo = new UserRegisteringModel(profile.Id, profile.FirstName, profile.LastName, profile.Email);
        var tokenValue = _jwtTokenGenerator.GenerateToken(userInfo);
        return new JwtTokenDto(tokenValue);
    }
}