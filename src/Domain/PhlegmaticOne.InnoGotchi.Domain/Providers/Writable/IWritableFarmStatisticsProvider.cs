using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableFarmStatisticsProvider
{
    Task<FarmStatistics> ProcessFeedingAsync(Guid profileId, CancellationToken cancellationToken = new());
    Task<FarmStatistics> ProcessDrinkingAsync(Guid profileId, CancellationToken cancellationToken = new());
}