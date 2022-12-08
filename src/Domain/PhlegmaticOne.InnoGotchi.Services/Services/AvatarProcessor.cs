using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Services;

namespace PhlegmaticOne.InnoGotchi.Services.Services;

public class AvatarProcessor : IAvatarProcessor
{
    private readonly IDefaultAvatarService _defaultAvatarService;

    public AvatarProcessor(IDefaultAvatarService defaultAvatarService) => _defaultAvatarService = defaultAvatarService;

    public async Task<Avatar> ProcessAvatarAsync(Avatar? avatar, CancellationToken cancellationToken = new())
    {
        if (avatar is not null && avatar.AvatarData.Any())
        {
            return avatar;
        }

        return new Avatar
        {
            AvatarData = await _defaultAvatarService.GetDefaultAvatarDataAsync(cancellationToken)
        };
    }

    public Task<byte[]> ProcessAvatarDataAsync(byte[]? avatarData, CancellationToken cancellationToken = new())
    {
        if (avatarData is not null && avatarData.Any())
        {
            return Task.FromResult(avatarData);
        }

        return _defaultAvatarService.GetDefaultAvatarDataAsync(cancellationToken);
    }
}