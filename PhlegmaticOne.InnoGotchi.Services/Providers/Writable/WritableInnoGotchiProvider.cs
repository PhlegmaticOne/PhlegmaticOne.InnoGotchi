using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.UnitOfWork.Interfaces;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableInnoGotchiProvider : IWritableInnoGotchiesProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInnoGotchiSignsUpdateService _innoGotchiSignsUpdateService;
    private readonly ITimeService _timeService;

    public WritableInnoGotchiProvider(IUnitOfWork unitOfWork, 
        IInnoGotchiSignsUpdateService innoGotchiSignsUpdateService,
        ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiSignsUpdateService = innoGotchiSignsUpdateService;
        _timeService = timeService;
    }

    public async Task<InnoGotchiModel> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto)
    {
        var created = await CreateInnoGotchi(createInnoGotchiDto);
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var createdInnoGotchi = await repository.CreateAsync(created);
        return createdInnoGotchi;
    }

    public Task DrinkAsync(Guid petId) =>
        ProcessPetUpdating(petId, pet =>
        {
            pet.ThirstyLevel = ThirstyLevel.Full;
            pet.LastDrinkTime = _timeService.Now();
        });

    public Task FeedAsync(Guid petId) =>
        ProcessPetUpdating(petId, pet =>
        {
            pet.HungerLevel = HungerLevel.Full;
            pet.LastFeedTime = _timeService.Now();
        });

    public Task SynchronizeSignsAsync(Guid petId) =>
        ProcessPetUpdating(petId, SynchronizeAction);

    public async Task SynchronizeSignsForAllInFarmAsync(Guid farmId)
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pets = await repository.GetAllAsync(predicate: p => p.Farm.Id == farmId);
        await repository.UpdateRangeAsync(pets, SynchronizeAction);
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

        if (currentPetAge < pet.Age)
        {
            pet.AgeUpdatedAt = now;
        }

        if (_innoGotchiSignsUpdateService.IsDeadNow(pet.HungerLevel, pet.ThirstyLevel, pet.Age))
        {
            pet.DeadSince = now;
        }
    }


    private async Task ProcessPetUpdating(Guid petId, Action<InnoGotchiModel> updateAction)
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pet = await repository.GetByIdOrDefaultAsync(petId);
        await repository.UpdateAsync(pet!, updateAction);
    }

    private async Task<InnoGotchiModel> CreateInnoGotchi(IdentityModel<CreateInnoGotchiDto> from)
    {
        var entity = from.Entity;
        var componentsToCreate = entity.Components;
        var components = await GetExistingComponents(componentsToCreate);
        var farm = await GetProfileFarm(from.ProfileId);
        var innoGotchiComponents = CreateModelComponents(componentsToCreate, components);
        var now = _timeService.Now();

        return new InnoGotchiModel
        {
            HungerLevel = HungerLevel.Normal,
            LastDrinkTime = now,
            LastFeedTime = now,
            Name = entity.Name,
            ThirstyLevel = ThirstyLevel.Normal,
            Components = innoGotchiComponents,
            Farm = farm,
            Age = 0,
            AgeUpdatedAt = now,
            HappinessDaysCount = 0,
            LiveSince = now
        };
    }

    private Task<IList<InnoGotchiComponent>> GetExistingComponents(List<InnoGotchiModelComponentDto> componentsToCreate)
    {
        var urls = componentsToCreate.Select(x => x.ImageUrl).ToList();
        var componentsRepository = _unitOfWork.GetRepository<InnoGotchiComponent>();
        return componentsRepository.GetAllAsync(predicate: x => urls.Contains(x.ImageUrl));
    }

    private Task<Farm> GetProfileFarm(Guid profileId)
    {
        var farmRepository = _unitOfWork.GetRepository<Farm>();
        return farmRepository.GetFirstOrDefaultAsync(x => x.Owner.Id == profileId)!;
    }

    private static List<InnoGotchiModelComponent> CreateModelComponents(List<InnoGotchiModelComponentDto> dtos, IEnumerable<InnoGotchiComponent> existingComponents) =>
        dtos.Join(existingComponents, on => on.ImageUrl, on => on.ImageUrl,
            (dto, component) => new InnoGotchiModelComponent
            {
                InnoGotchiComponent = component,
                TranslationX = dto.TranslationX,
                TranslationY = dto.TranslationY,
                ScaleX = dto.ScaleX,
                ScaleY = dto.ScaleY
            }).ToList();
}