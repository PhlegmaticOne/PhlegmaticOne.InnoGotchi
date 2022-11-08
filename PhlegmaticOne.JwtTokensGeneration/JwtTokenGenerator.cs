using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.JwtTokensGeneration.Options;

namespace PhlegmaticOne.JwtTokensGeneration;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IJwtOptions _jwtOptions;

    public JwtTokenGenerator(IJwtOptions jwtOptions) => _jwtOptions = jwtOptions;

    public string GenerateToken(string userName)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userName),
            new(JwtRegisteredClaimNames.Email, userName)
        };

        var securityKey = _jwtOptions.GetSecretKey();
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpirationDurationInMinutes),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}