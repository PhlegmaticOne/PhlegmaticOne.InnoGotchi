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

    public async Task<OperationResult> DrinkAsync(IdentityModel<IdDto> petIdModel)
    {
        var drinkResult = await _innoGotchiesProvider.DrinkAsync(petIdModel);

        if (drinkResult.IsSuccess == false)
        {
            return OperationResult.FromFail(drinkResult.ErrorMessage);
        }

        var farmStatisticsResult = await _farmStatisticsProvider.ProcessDrinkingAsync(petIdModel.ProfileId);

        if (farmStatisticsResult.IsSuccess == false)
        {
            return OperationResult.FromFail(farmStatisticsResult.ErrorMessage);
        }

        await _unitOfWork.SaveChangesAsync();

        return OperationResult.Success;
    }

    public async Task<OperationResult> FeedAsync(IdentityModel<IdDto> petIdModel)
    {
        var drinkResult = await _innoGotchiesProvider.FeedAsync(petIdModel);

        if (drinkResult.IsSuccess == false)
        {
            return OperationResult.FromFail(drinkResult.ErrorMessage);
        }

        var farmStatisticsResult = await _farmStatisticsProvider.ProcessFeedingAsync(petIdModel.ProfileId);

        if (farmStatisticsResult.IsSuccess == false)
        {
            return OperationResult.FromFail(farmStatisticsResult.ErrorMessage);
        }

        await _unitOfWork.SaveChangesAsync();

        return OperationResult.Success;
    }
}