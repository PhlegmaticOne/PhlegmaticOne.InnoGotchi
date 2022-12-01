using PhlegmaticOne.InnoGotchi.Shared.JwtToken;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;

namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface IJwtTokenGenerationService
{
    JwtTokenDto GenerateJwtToken(AuthorizedProfileDto profile);
}