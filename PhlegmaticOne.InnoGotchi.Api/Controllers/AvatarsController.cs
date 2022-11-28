using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class AvatarsController : IdentityController
{
    private readonly IReadableAvatarProvider _readableAvatarProvider;

    public AvatarsController(IReadableAvatarProvider readableAvatarProvider) => 
        _readableAvatarProvider = readableAvatarProvider;

    [HttpGet]
    public async Task<OperationResult<byte[]>> GetAuthorized()
    {
        var avatar = await _readableAvatarProvider.GetAvatarAsync(ProfileId());
        return OperationResult.FromSuccess(avatar.AvatarData);
    }
}