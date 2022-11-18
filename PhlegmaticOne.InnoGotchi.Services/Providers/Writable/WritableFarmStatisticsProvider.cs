using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableFarmStatisticsProvider : IWritableFarmStatisticsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public WritableFarmStatisticsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<FarmStatistics>> ProcessFeedingAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<FarmStatistics>();
        var farmStatistics = await repository.GetFirstOrDefaultAsync(x => x.Farm.OwnerId == profileId);
        var now = DateTime.Now;

        var updated = await repository.UpdateAsync(farmStatistics!, statistics =>
        {
            statistics.AverageFeedTime = CalculateNewAverage(statistics.AverageFeedTime, statistics.LastFeedTime, now,
                statistics.TotalFeedingsCount);
            statistics.TotalFeedingsCount += 1;
            statistics.LastFeedTime = now;
        });

        return OperationResult.FromSuccess(updated);
    }

    public async Task<OperationResult<FarmStatistics>> ProcessDrinkingAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<FarmStatistics>();
        var farmStatistics = await repository.GetFirstOrDefaultAsync(x => x.Farm.OwnerId == profileId);
        var now = DateTime.Now;

        var updated = await repository.UpdateAsync(farmStatistics!, statistics =>
        {
            statistics.AverageDrinkTime = CalculateNewAverage(statistics.AverageDrinkTime, statistics.LastDrinkTime, now,
                statistics.TotalDrinkingsCount);
            statistics.TotalDrinkingsCount += 1;
            statistics.LastDrinkTime = now;
        });

        return OperationResult.FromSuccess(updated);
    }

    private static TimeSpan CalculateNewAverage(TimeSpan currentAverage, DateTime lastActionTime, DateTime now, int currentActionsCount)
    {
        var difference = now - lastActionTime;
        return (currentAverage + difference) / (currentActionsCount + 1);
    }
}