using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Services.Commands.Synchronization.Base;
using PhlegmaticOne.OperationResults.Mediatr;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Commands.Synchronization;

public class SynchronizeInnoGotchiesInProfileFarmCommand : IdentityOperationResultCommandBase
{
    public SynchronizeInnoGotchiesInProfileFarmCommand(Guid profileId) : base(profileId)
    {
    }
}

public class SynchronizeInnoGotchiesInProfileFarmCommandHandler :
    SynchronizeFarmCommandHandlerBase<SynchronizeInnoGotchiesInProfileFarmCommand>
{
    public SynchronizeInnoGotchiesInProfileFarmCommandHandler(IUnitOfWork unitOfWork,
        IInnoGotchiesSynchronizationProvider innoGotchiesSynchronizationProvider) :
        base(unitOfWork, innoGotchiesSynchronizationProvider)
    {
    }

    protected override Task<Guid> GetFarmId(SynchronizeInnoGotchiesInProfileFarmCommand request,
        CancellationToken cancellationToken)
    {
        return UnitOfWork.GetRepository<Farm>()
            .GetFirstOrDefaultAsync(
                predicate: x => x.OwnerId == request.ProfileId,
                selector: x => x.Id,
                cancellationToken: cancellationToken);
    }
}