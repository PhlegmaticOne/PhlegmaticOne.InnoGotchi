namespace PhlegmaticOne.InnoGotchi.Api.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userName);
}