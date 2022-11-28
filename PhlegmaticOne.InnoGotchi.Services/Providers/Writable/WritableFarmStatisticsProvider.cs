using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableFarmStatisticsProvider : IWritableFarmStatisticsProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeService _timeService;

    public WritableFarmStatisticsProvider(IUnitOfWork unitOfWork, ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _timeService = timeService;
    }

    public async Task<FarmStatistics> ProcessFeedingAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<FarmStatistics>();
        var farmStatistics = await repository.GetFirstOrDefaultAsync(x => x.Farm.Owner.Id == profileId);
        var now = _timeService.Now();

        var updated = await repository.UpdateAsync(farmStatistics!, statistics =>
        {
            statistics.AverageFeedTime = CalculateNewAverage(statistics.AverageFeedTime, statistics.LastFeedTime, now,
                statistics.TotalFeedingsCount);
            statistics.TotalFeedingsCount += 1;
            statistics.LastFeedTime = now;
        });

        return updated;
    }

    public async Task<FarmStatistics> ProcessDrinkingAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<FarmStatistics>();
        var farmStatistics = await repository.GetFirstOrDefaultAsync(x => x.Farm.Owner.Id == profileId);
        var now = _timeService.Now();

        var updated = await repository.UpdateAsync(farmStatistics!, statistics =>
        {
            statistics.AverageDrinkTime = CalculateNewAverage(statistics.AverageDrinkTime, statistics.LastDrinkTime, now,
                statistics.TotalDrinkingsCount);
            statistics.TotalDrinkingsCount += 1;
            statistics.LastDrinkTime = now;
        });

        return updated;
    }

    private static TimeSpan CalculateNewAverage(TimeSpan currentAverage, DateTime lastActionTime, DateTime now, int currentActionsCount)
    {
        var difference = now - lastActionTime;
        return (currentAverage + difference) / (currentActionsCount + 1);
    }
}