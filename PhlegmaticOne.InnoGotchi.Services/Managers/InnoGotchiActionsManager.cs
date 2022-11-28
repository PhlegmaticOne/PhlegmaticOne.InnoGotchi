using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class InnoGotchiActionsManager : IInnoGotchiActionsManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableInnoGotchiesProvider _innoGotchiesProvider;
    private readonly IWritableFarmStatisticsProvider _farmStatisticsProvider;

    public InnoGotchiActionsManager(IUnitOfWork unitOfWork,
        IWritableInnoGotchiesProvider innoGotchiesProvider,
        IWritableFarmStatisticsProvider farmStatisticsProvider)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiesProvider = innoGotchiesProvider;
        _farmStatisticsProvider = farmStatisticsProvider;
    }

    public Task<OperationResult> DrinkAsync(IdentityModel<IdDto> petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.DrinkAsync(petIdModel);

            await _farmStatisticsProvider.ProcessDrinkingAsync(petIdModel.ProfileId);

            await _unitOfWork.SaveChangesAsync();
        });
    }

    public Task<OperationResult> FeedAsync(IdentityModel<IdDto> petIdModel)
    {
        return _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await _innoGotchiesProvider.FeedAsync(petIdModel);

            await _farmStatisticsProvider.ProcessFeedingAsync(petIdModel.ProfileId);

            await _unitOfWork.SaveChangesAsync();
        });
    }
}