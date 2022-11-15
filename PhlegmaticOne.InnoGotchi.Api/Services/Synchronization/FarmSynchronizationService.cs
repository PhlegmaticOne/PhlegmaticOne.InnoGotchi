using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Api.Services.InnoGotchiPolicies;
using PhlegmaticOne.InnoGotchi.Api.Services.Time;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Synchronization;

public class FarmSynchronizationService : IFarmSynchronizationService
{
    private readonly IDataService _dataService;
    private readonly IInnoGotchiActionsPolicy _innoGotchiPolicy;
    private readonly ITimeProvider _timeProvider;

    public FarmSynchronizationService(IDataService dataService, IInnoGotchiActionsPolicy innoGotchiPolicy, ITimeProvider timeProvider)
    {
        _dataService = dataService;
        _innoGotchiPolicy = innoGotchiPolicy;
        _timeProvider = timeProvider;
    }

    public async Task<FarmStatisticsDto> SynchronizeSinceLastTimeAsync(Guid profileId)
    {
        var innoGotchies = await GetPets(profileId);
        SynchronizePetsWithTime(innoGotchies);
        await _dataService.SaveChangesAsync();
        return new FarmStatisticsDto();
    }

    private void SynchronizePetsWithTime(IList<InnoGotchiModel> innoGotchies)
    {
        var currentTime = _timeProvider.Now();

        foreach (var innoGotchiModel in innoGotchies)
        {
            innoGotchiModel.HungerLevel = SynchronizationHelper.SynchronizeEnumWithTime(
                innoGotchiModel.HungerLevel, currentTime, innoGotchiModel.LastFeedTime,
                _innoGotchiPolicy.TimeToIncreaseHungerLevel);

            innoGotchiModel.ThirstyLevel = SynchronizationHelper.SynchronizeEnumWithTime(
                innoGotchiModel.ThirstyLevel, currentTime, innoGotchiModel.LastDrinkTime,
                _innoGotchiPolicy.TimeToIncreaseThirstLevel);

            innoGotchiModel.Age = SynchronizationHelper.IncreaseUntilNotSynchronizedWithTime(
                innoGotchiModel.Age, currentTime, innoGotchiModel.AgeUpdatedAt, 
                _innoGotchiPolicy.TimeToIncreaseAge);

            innoGotchiModel.AgeUpdatedAt = currentTime;
        }
    }

    private Task<IList<InnoGotchiModel>> GetPets(Guid profileId) =>
        _dataService.GetDataRepository<InnoGotchiModel>()
            .GetAllAsync(predicate: p => p.Farm.OwnerId == profileId);
}