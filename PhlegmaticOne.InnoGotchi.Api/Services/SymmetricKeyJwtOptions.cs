﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PhlegmaticOne.InnoGotchi.Api.Services;

public class SymmetricKeyJwtOptions : IJwtOptions
{
    private readonly string _secretKey;

    public SymmetricKeyJwtOptions(string issuer, string audience, int expirationDurationInMinutes, string secretKey)
    {
        _secretKey = secretKey;
        Issuer = issuer;
        Audience = audience;
        ExpirationDurationInMinutes = expirationDurationInMinutes;
    }
    public string Issuer { get; }
    public string Audience { get; }
    public int ExpirationDurationInMinutes { get; }

    public SecurityKey GetSecretKey()
    {
        var bytes = Encoding.UTF8.GetBytes(_secretKey);
        return new SymmetricSecurityKey(bytes);
    }
}