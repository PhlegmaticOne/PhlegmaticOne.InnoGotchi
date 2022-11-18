using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableFarmStatisticsProvider
{
    Task<OperationResult<FarmStatistics>> ProcessFeedingAsync(Guid profileId);
    Task<OperationResult<FarmStatistics>> ProcessDrinkingAsync(Guid profileId);
}