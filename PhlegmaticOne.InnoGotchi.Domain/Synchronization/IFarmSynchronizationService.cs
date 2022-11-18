using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Domain.Synchronization;

public interface IFarmSynchronizationService
{
    Task<FarmStatisticsDto> SynchronizeSinceLastTimeAsync(Guid profileId);
}