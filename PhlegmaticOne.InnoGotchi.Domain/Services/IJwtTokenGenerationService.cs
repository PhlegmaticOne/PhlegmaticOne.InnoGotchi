using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.JwtToken;

namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface IJwtTokenGenerationService
{
    JwtTokenDto GenerateJwtToken(UserProfile userProfile);
}