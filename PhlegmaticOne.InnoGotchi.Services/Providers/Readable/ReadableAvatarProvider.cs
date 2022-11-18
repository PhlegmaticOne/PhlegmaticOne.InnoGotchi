using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableAvatarProvider : IReadableAvatarProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableAvatarProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<Avatar>> GetAvatarAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<Avatar>();
        var avatar = await repository.GetFirstOrDefaultAsync(x => x.UserProfileId == profileId);
        return OperationResult.FromSuccess(avatar)!;
    }
}