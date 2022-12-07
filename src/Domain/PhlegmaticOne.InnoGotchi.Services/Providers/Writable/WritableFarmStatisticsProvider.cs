using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.UnitOfWork.Interfaces;
using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Services.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableFarmStatisticsProvider : IWritableFarmStatisticsProvider
{
    private readonly ITimeService _timeService;
    private readonly IUnitOfWork _unitOfWork;

    public WritableFarmStatisticsProvider(IUnitOfWork unitOfWork, ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _timeService = timeService;
    }

    public async Task<FarmStatistics> ProcessFeedingAsync(Guid profileId, CancellationToken cancellationToken = new())
    {
        var farmStatistics = await GetStatistics(profileId, cancellationToken);
        var now = _timeService.Now();
        var repository = _unitOfWork.GetRepository<FarmStatistics>();

        var updated = await repository.UpdateAsync(farmStatistics, statistics =>
        {
            statistics.AverageFeedTime = AverageCalculator.CalculateNewAverage(
                statistics.AverageFeedTime, statistics.LastFeedTime, now, statistics.TotalFeedingsCount);
            statistics.TotalFeedingsCount += 1;
            statistics.LastFeedTime = now;
        }, cancellationToken);

        return updated;
    }

    public async Task<FarmStatistics> ProcessDrinkingAsync(Guid profileId, CancellationToken cancellationToken = new())
    {
        var farmStatistics = await GetStatistics(profileId, cancellationToken);
        var now = _timeService.Now();
        var repository = _unitOfWork.GetRepository<FarmStatistics>();

        var updated = await repository.UpdateAsync(farmStatistics, statistics =>
        {
            statistics.AverageDrinkTime = AverageCalculator.CalculateNewAverage(
                    statistics.AverageDrinkTime, statistics.LastDrinkTime, now, statistics.TotalDrinkingsCount);
            statistics.TotalDrinkingsCount += 1;
            statistics.LastDrinkTime = now;
        }, cancellationToken);

        return updated;
    }

    private async Task<FarmStatistics> GetStatistics(Guid profileId, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.GetRepository<FarmStatistics>();
        var farmStatistics = await repository.GetFirstOrDefaultAsync(x => x.Farm.Owner.Id == profileId, cancellationToken: cancellationToken);

        if (farmStatistics is null)
        {
            throw new DomainException(AppErrorMessages.ProfileDoesNotHaveFarmStatistics);
        }
        return farmStatistics;
    }
}