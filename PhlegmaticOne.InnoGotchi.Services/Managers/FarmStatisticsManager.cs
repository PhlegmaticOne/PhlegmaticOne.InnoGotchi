using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class FarmStatisticsManager : IFarmStatisticsManager
{
    private readonly IReadableFarmProvider _readableFarmProvider;
    private readonly IReadableFarmStatisticsProvider _readableFarmStatisticsProvider;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;

    public FarmStatisticsManager(
        IReadableFarmProvider readableFarmProvider,
        IReadableFarmStatisticsProvider readableFarmStatisticsProvider,
        IReadableInnoGotchiProvider readableInnoGotchiProvider)
    {
        _readableFarmProvider = readableFarmProvider;
        _readableFarmStatisticsProvider = readableFarmStatisticsProvider;
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
    }

    public async Task<OperationResult<PreviewFarmStatisticsDto>> BuildForProfileAsync(Guid profileId)
    {
        var result = await _readableFarmProvider.GetFarmAsync(profileId);
        var farm = result.Result!;
        var petsCount = await _readableFarmProvider.GetPetsCountInFarmAsync(farm.Id);

        return OperationResult.FromSuccess(new PreviewFarmStatisticsDto
        {
            FarmName = farm.Name,
            FarmId = farm.Id,
            PetsCount = petsCount.Result
        });
    }

    public async Task<OperationResult<DetailedFarmStatisticsDto>> BuildDetailedForProfileAsync(Guid profileId)
    {
        var result = await _readableFarmProvider.GetFarmAsync(profileId);
        var farm = result.Result!;

        var farmStatisticsResult = await _readableFarmStatisticsProvider.GetForFarmAsync(farm.Id);
        var farmStatistics = farmStatisticsResult.Result!;

        var innoGotchiesResult = await _readableInnoGotchiProvider.GetAllAsync(farm.Id);
        var innoGotchies = innoGotchiesResult.Result!;

        var alivePets = GetAlivePets(innoGotchies).ToList();
        var deadPets = GetDeadPets(innoGotchies).ToList();

        return OperationResult.FromSuccess(new DetailedFarmStatisticsDto
        {
            FarmName = farm.Name,
            FarmId = farm.Id,
            PetsCount = innoGotchies.Count,
            AverageFeedingPeriod = farmStatistics.AverageFeedTime,
            AverageThirstQuenchingPeriod = farmStatistics.AverageDrinkTime,
            AverageHappinessDaysCount = GetAverageHappinessDaysCount(innoGotchies),
            AlivePetsCount = alivePets.Count,
            DeadPetsCount = deadPets.Count,
            AverageAlivePetsAge = GetAverageAge(alivePets),
            AverageDeadPetsAge = GetAverageAge(deadPets)
        });
    }

    public Task<OperationResult<IList<PreviewFarmStatisticsDto>>> BuildForCollaboratedProfilesAsync(Guid profileId)
    {
        throw new NotImplementedException();
    }

    private static double GetAverageHappinessDaysCount(IList<InnoGotchiModel> innoGotchies) => 
        innoGotchies.Count == 0 ? 0 : innoGotchies.Average(x => x.HappinessDaysCount);
    private static double GetAverageAge(IList<InnoGotchiModel> innoGotchies) => 
        innoGotchies.Count == 0 ? 0 : innoGotchies.Average(x => x.Age);
    private static IEnumerable<InnoGotchiModel> GetAlivePets(IList<InnoGotchiModel> innoGotchies) => 
        innoGotchies.Where(x => x.DeadSince == DateTime.MinValue);
    private static IEnumerable<InnoGotchiModel> GetDeadPets(IList<InnoGotchiModel> innoGotchies) =>
        innoGotchies.Where(x => x.DeadSince != DateTime.MinValue);
}