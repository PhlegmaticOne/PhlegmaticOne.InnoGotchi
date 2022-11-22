using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableAvatarProvider : IReadableAvatarProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDefaultAvatarService _defaultAvatarService;

    public ReadableAvatarProvider(IUnitOfWork unitOfWork, IDefaultAvatarService defaultAvatarService)
    {
        _unitOfWork = unitOfWork;
        _defaultAvatarService = defaultAvatarService;
    }

    public async Task<OperationResult<Avatar>> GetAvatarAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<Avatar>();
        var avatar = await repository.GetFirstOrDefaultAsync(x => x.UserProfileId == profileId);

        if (avatar?.AvatarData.Any() == false)
        {
            avatar.AvatarData = await _defaultAvatarService.GetDefaultAvatarDataAsync();
        }

        return OperationResult.FromSuccess(avatar)!;
    }
}