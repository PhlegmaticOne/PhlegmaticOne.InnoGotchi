using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.JwtTokensGeneration.Extensions;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers.Base;

public class IdentityController : ControllerBase
{
    protected Guid ProfileId() => User.GetUserId();
}