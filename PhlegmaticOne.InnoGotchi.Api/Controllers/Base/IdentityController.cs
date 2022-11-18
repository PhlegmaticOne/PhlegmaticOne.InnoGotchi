using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.JwtTokensGeneration.Extensions;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers.Base;

public class IdentityController : ControllerBase
{
    protected Guid ProfileId() => User.GetUserId();

    protected IdentityModel<T> IdentityModel<T>(T model) => new()
    {
        Entity = model,
        ProfileId = ProfileId()
    };
}