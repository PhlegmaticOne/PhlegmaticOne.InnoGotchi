﻿using System.Security.Claims;
using PhlegmaticOne.JwtTokensGeneration.Helpers;

namespace PhlegmaticOne.JwtTokensGeneration.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var claimValue = claimsPrincipal.Claims.First(x => x.Type == CustomJwtClaimNames.UserId).Value;
        return Guid.Parse(claimValue);
    }
}