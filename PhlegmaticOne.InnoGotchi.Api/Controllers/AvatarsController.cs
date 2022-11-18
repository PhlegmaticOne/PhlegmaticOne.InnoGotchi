using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhlegmaticOne.InnoGotchi.Api.Controllers.Base;
using PhlegmaticOne.InnoGotchi.Api.Services;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class AvatarsController : IdentityController
{
    private readonly IReadableAvatarProvider _readableAvatarProvider;
    private readonly IDefaultAvatarService _avatarConvertingService;

    public AvatarsController(IReadableAvatarProvider readableAvatarProvider, IDefaultAvatarService avatarConvertingService)
    {
        _readableAvatarProvider = readableAvatarProvider;
        _avatarConvertingService = avatarConvertingService;
    }

    [HttpGet]
    public async Task<OperationResult<byte[]>> Get()
    {
        var avatar = await _readableAvatarProvider.GetAvatarAsync(ProfileId());

        var result = avatar.Result;

        if (result?.AvatarData.Any() == false)
        {
            var defaultAvatar = await _avatarConvertingService.GetDefaultAvatarDataAsync();
            return OperationResult.FromSuccess(defaultAvatar);
        }

        return OperationResult.FromSuccess(result!.AvatarData);
    }
}