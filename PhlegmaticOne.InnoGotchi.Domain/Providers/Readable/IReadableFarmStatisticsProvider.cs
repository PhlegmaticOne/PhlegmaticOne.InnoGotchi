using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableFarmStatisticsProvider
{
    Task<OperationResult<FarmStatistics>> GetForFarmAsync(Guid farmId);
}