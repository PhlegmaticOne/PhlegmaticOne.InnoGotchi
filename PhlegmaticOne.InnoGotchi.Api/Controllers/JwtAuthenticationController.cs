using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhlegmaticOne.InnoGotchi.Api.Helpers;
using PhlegmaticOne.InnoGotchi.Data.Core.Services;
using PhlegmaticOne.InnoGotchi.Shared.Dtos;
using PhlegmaticOne.PasswordHasher.Base;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class JwtAuthenticationController : Controller
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUsersDataService _usersDataService;

    public JwtAuthenticationController(IPasswordHasher passwordHasher, IUsersDataService usersDataService)
    {
        _passwordHasher = passwordHasher;
        _usersDataService = usersDataService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<JwtTokenDto> Authenticate([FromBody] UserCredentials userCredentials)
    {
        var passwordHash = _passwordHasher.Hash(userCredentials.Password);
        var email = userCredentials.Email;

        if (await _usersDataService.IsExistsAsync(email, passwordHash) == false)
        {
            return JwtTokenDto.Failed;
        }

        var tokenValue = CreateJwtToken(email);

        return JwtTokenDto.FromToken(tokenValue);
    }

    private string CreateJwtToken(string email)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, email),
            new(JwtRegisteredClaimNames.Email, email),
        };

        var keyBytes = JwtAuthenticationHelper.GetSecretBytes();
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            JwtAuthenticationHelper.Issuer,
            JwtAuthenticationHelper.Audience,
            claims,
            notBefore: DateTime.Now,
            expires: DateTime.Now.AddMinutes(1),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}