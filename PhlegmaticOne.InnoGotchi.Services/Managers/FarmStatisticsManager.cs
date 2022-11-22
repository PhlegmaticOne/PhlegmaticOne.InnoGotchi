using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.InnoGotchi.Shared.FarmStatistics;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class FarmStatisticsManager : IFarmStatisticsManager
{
    private readonly IReadableCollaborationsProvider _readableCollaborationsProvider;
    private readonly IReadableProfileProvider _readableProfileProvider;
    private readonly IReadableFarmProvider _readableFarmProvider;
    private readonly IReadableFarmStatisticsProvider _readableFarmStatisticsProvider;
    private readonly IReadableInnoGotchiProvider _readableInnoGotchiProvider;

    public FarmStatisticsManager(
        IReadableCollaborationsProvider readableCollaborationsProvider,
        IReadableProfileProvider readableProfileProvider,
        IReadableFarmProvider readableFarmProvider,
        IReadableFarmStatisticsProvider readableFarmStatisticsProvider,
        IReadableInnoGotchiProvider readableInnoGotchiProvider)
    {
        _readableCollaborationsProvider = readableCollaborationsProvider;
        _readableProfileProvider = readableProfileProvider;
        _readableFarmProvider = readableFarmProvider;
        _readableFarmStatisticsProvider = readableFarmStatisticsProvider;
        _readableInnoGotchiProvider = readableInnoGotchiProvider;
    }

    public async Task<OperationResult<PreviewFarmStatisticsDto>> BuildForProfileAsync(Guid profileId)
    {
        var farmResult = await _readableFarmProvider.GetFarmAsync(profileId);
        var farm = farmResult.Result!;

        var profileResult = await _readableProfileProvider.GetExistingOrDefaultAsync(profileId);
        var profile = profileResult.Result!;

        var petsCount = await _readableFarmProvider.GetPetsCountInFarmAsync(farm.Id);

        return OperationResult.FromSuccess(new PreviewFarmStatisticsDto
        {
            FarmName = farm.Name,
            FarmId = farm.Id,
            PetsCount = petsCount.Result,
            ProfileEmail = profile.User.Email,
            ProfileFirstName = profile.FirstName,
            ProfileLastName = profile.LastName
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

    public async Task<OperationResult<IList<PreviewFarmStatisticsDto>>> BuildForCollaboratedProfilesAsync(Guid profileId)
    {
        var collaborationsResult = await _readableCollaborationsProvider.GetCollaboratedUsersAsync(profileId);
        var collaboratedUsers = collaborationsResult.Result!;

        IList<PreviewFarmStatisticsDto> result = new List<PreviewFarmStatisticsDto>();

        foreach (var collaboratedUser in collaboratedUsers)
        {
            var statisticsResult = await BuildForProfileAsync(collaboratedUser.Id);
            result.Add(statisticsResult.Result!);
        }

        return OperationResult.FromSuccess(result);
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