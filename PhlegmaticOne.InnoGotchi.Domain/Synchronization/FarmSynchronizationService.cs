using PhlegmaticOne.InnoGotchi.Domain.InnoGotchiPolicies;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Domain.Synchronization;

//public class FarmSynchronizationService : IFarmSynchronizationService
//{
//    private readonly IUnitOfWork _dataService;
//    private readonly IInnoGotchiActionsPolicy _innoGotchiPolicy;

//    public FarmSynchronizationService(IUnitOfWork dataService, IInnoGotchiActionsPolicy innoGotchiPolicy)
//    {
//        _dataService = dataService;
//        _innoGotchiPolicy = innoGotchiPolicy;
//    }

//    public async Task<FarmStatisticsDto> SynchronizeSinceLastTimeAsync(Guid profileId)
//    {
//        var innoGotchies = await GetPets(profileId);
//        SynchronizePetsWithTime(innoGotchies);
//        await _dataService.SaveChangesAsync();
//        return new FarmStatisticsDto();
//    }

//    private void SynchronizePetsWithTime(IList<InnoGotchiModel> innoGotchies)
//    {
//        var currentTime = DateTime.Now;

//        foreach (var innoGotchiModel in innoGotchies)
//        {
//            innoGotchiModel.HungerLevel = SynchronizationHelper.SynchronizeEnumWithTime(
//                innoGotchiModel.HungerLevel, currentTime, innoGotchiModel.LastFeedTime,
//                _innoGotchiPolicy.TimeToIncreaseHungerLevel);

//            innoGotchiModel.ThirstyLevel = SynchronizationHelper.SynchronizeEnumWithTime(
//                innoGotchiModel.ThirstyLevel, currentTime, innoGotchiModel.LastDrinkTime,
//                _innoGotchiPolicy.TimeToIncreaseThirstLevel);

//            innoGotchiModel.Age = SynchronizationHelper.IncreaseUntilNotSynchronizedWithTime(
//                innoGotchiModel.Age, currentTime, innoGotchiModel.AgeUpdatedAt, 
//                _innoGotchiPolicy.TimeToIncreaseAge);

//            innoGotchiModel.AgeUpdatedAt = currentTime;
//        }
//    }

//    private Task<IList<InnoGotchiModel>> GetPets(Guid profileId) =>
//        _dataService.GetDataRepository<InnoGotchiModel>()
//            .GetAllAsync(predicate: p => p.Farm.OwnerId == profileId);
//}