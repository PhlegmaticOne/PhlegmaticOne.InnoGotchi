using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class InnoGotchiesSynchronizationProvider : IInnoGotchiesSynchronizationProvider
{
    private readonly IInnoGotchiSignsUpdateService _innoGotchiSignsUpdateService;
    private readonly ITimeService _timeService;
    private readonly IUnitOfWork _unitOfWork;

    public InnoGotchiesSynchronizationProvider(IUnitOfWork unitOfWork,
        IInnoGotchiSignsUpdateService innoGotchiSignsUpdateService,
        ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiSignsUpdateService = innoGotchiSignsUpdateService;
        _timeService = timeService;
    }

    public async Task SynchronizeAllPetsAsync(CancellationToken cancellationToken = new())
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pets = await repository.GetAllAsync(cancellationToken: cancellationToken);

        if (pets.Any() == false)
        {
            return;
        }

        await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await repository.UpdateRangeAsync(pets, SynchronizeAction, cancellationToken);
        });
    }

    public async Task SynchronizePetAsync(Guid petId, CancellationToken cancellationToken = new())
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pet = await repository.GetByIdOrDefaultAsync(petId, cancellationToken: cancellationToken);

        if (pet is null) return;

        await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await repository.UpdateAsync(pet, SynchronizeAction, cancellationToken);
        });
    }

    public async Task SynchronizePetsInFarmAsync(Guid farmId, CancellationToken cancellationToken = new())
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pets = await repository.GetAllAsync(
            predicate: p => p.FarmId == farmId,
            cancellationToken: cancellationToken);

        if (pets.Any() == false)
        {
            return;
        }

        await _unitOfWork.ResultFromExecutionInTransaction(async () =>
        {
            await repository.UpdateRangeAsync(pets, SynchronizeAction, cancellationToken);
        });
    }

    private void SynchronizeAction(InnoGotchiModel pet)
    {
        var now = _timeService.Now();
        var currentPetAge = pet.Age;

        pet.HungerLevel = _innoGotchiSignsUpdateService.TryIncreaseHungerLevel(pet.HungerLevel, pet.LastFeedTime);
        pet.ThirstyLevel = _innoGotchiSignsUpdateService.TryIncreaseThirstLevel(pet.ThirstyLevel, pet.LastDrinkTime);
        pet.Age = _innoGotchiSignsUpdateService.TryIncreaseAge(pet.Age, pet.AgeUpdatedAt);
        pet.HappinessDaysCount =
            _innoGotchiSignsUpdateService.CalculateHappinessDaysCount(pet.HungerLevel, pet.ThirstyLevel, pet.LiveSince);

        if (currentPetAge < pet.Age) pet.AgeUpdatedAt = now;

        if (_innoGotchiSignsUpdateService.IsDeadNow(pet.HungerLevel, pet.ThirstyLevel, pet.Age)) pet.DeadSince = now;
    }
}