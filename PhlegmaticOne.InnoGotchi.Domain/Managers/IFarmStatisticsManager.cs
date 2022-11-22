using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IFarmStatisticsManager
{
    Task<OperationResult<PreviewFarmStatisticsDto>> BuildForProfileAsync(Guid profileId);
    Task<OperationResult<DetailedFarmStatisticsDto>> BuildDetailedForProfileAsync(Guid profileId);
    Task<OperationResult<IList<PreviewFarmStatisticsDto>>> BuildForCollaboratedProfilesAsync(Guid profileId);
}