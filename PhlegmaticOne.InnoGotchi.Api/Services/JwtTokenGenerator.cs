using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.InnoGotchi.Api.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PhlegmaticOne.InnoGotchi.Api.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly TimeSpan _expirationDuration;

    public JwtTokenGenerator(TimeSpan expirationDuration) => 
        _expirationDuration = expirationDuration;

    public string GenerateToken(string userName)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userName),
            new(JwtRegisteredClaimNames.Email, userName)
        };

        var keyBytes = JwtAuthenticationHelper.GetSecretBytes();
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            JwtAuthenticationHelper.Issuer,
            JwtAuthenticationHelper.Audience,
            claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.Add(_expirationDuration),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}