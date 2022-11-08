using Microsoft.IdentityModel.Tokens;

namespace PhlegmaticOne.InnoGotchi.Api.Services;

public interface IJwtOptions
{
    string Issuer { get; }
    string Audience { get; }
    int ExpirationDurationInMinutes { get; }
    SecurityKey GetSecretKey();
}