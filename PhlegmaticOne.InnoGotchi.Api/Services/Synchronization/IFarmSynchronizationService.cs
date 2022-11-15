using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Synchronization;

public interface IFarmSynchronizationService
{
    Task<FarmStatisticsDto> SynchronizeSinceLastTimeAsync(Guid profileId);
}