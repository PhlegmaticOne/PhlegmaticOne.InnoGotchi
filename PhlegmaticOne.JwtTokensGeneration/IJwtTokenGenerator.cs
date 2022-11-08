namespace PhlegmaticOne.JwtTokensGeneration;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userName);
}