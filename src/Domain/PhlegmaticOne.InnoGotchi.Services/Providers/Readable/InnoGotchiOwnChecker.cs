using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class InnoGotchiOwnChecker : IInnoGotchiOwnChecker
{
    private readonly IUnitOfWork _unitOfWork;

    public InnoGotchiOwnChecker(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<bool> IsBelongAsync(Guid profileId, Guid petId,
        CancellationToken cancellationToken = new())
    {
        var innoGotchiesRepository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var collaborationsRepository = _unitOfWork.GetRepository<Collaboration>();

        var isPetBelongToProfile = await innoGotchiesRepository.ExistsAsync(x =>
            x.Farm.OwnerId == profileId && x.Id == petId, cancellationToken);

        if (isPetBelongToProfile) return true;

        return await collaborationsRepository.ExistsAsync(
            x => x.UserProfileId == profileId &&
                 x.Farm.InnoGotchies.Select(i => i.Id).Contains(petId), cancellationToken);
    }
}