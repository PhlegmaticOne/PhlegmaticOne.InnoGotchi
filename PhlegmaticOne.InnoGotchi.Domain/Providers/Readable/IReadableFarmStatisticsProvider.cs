using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableFarmStatisticsProvider
{
    Task<FarmStatistics> GetForFarmAsync(Guid farmId);
}