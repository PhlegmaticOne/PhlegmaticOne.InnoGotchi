using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.UnitOfWork.Interfaces;

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

    public async Task<InnoGotchiModel> CreateAsync(Guid profileId, CreateInnoGotchiDto createInnoGotchiDto,
        CancellationToken cancellationToken = new())
    {
        var created = await CreateInnoGotchi(profileId, createInnoGotchiDto, cancellationToken);
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var createdInnoGotchi = await repository.CreateAsync(created, cancellationToken);
        return createdInnoGotchi;
    }

    public Task DrinkAsync(Guid petId, CancellationToken cancellationToken = new()) =>
        ProcessPetUpdating(petId, pet =>
        {
            pet.ThirstyLevel = ThirstyLevel.Full;
            pet.LastDrinkTime = _timeService.Now();
        }, cancellationToken);

    public Task FeedAsync(Guid petId, CancellationToken cancellationToken = new()) =>
        ProcessPetUpdating(petId, pet =>
        {
            pet.HungerLevel = HungerLevel.Full;
            pet.LastFeedTime = _timeService.Now();
        }, cancellationToken);


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


    private async Task ProcessPetUpdating(Guid petId, Action<InnoGotchiModel> updateAction,
        CancellationToken cancellationToken = new())
    {
        var repository = _unitOfWork.GetRepository<InnoGotchiModel>();
        var pet = await repository.GetByIdOrDefaultAsync(petId, cancellationToken: cancellationToken);
        await repository.UpdateAsync(pet!, updateAction, cancellationToken);
    }

    private async Task<InnoGotchiModel> CreateInnoGotchi(Guid profileId, CreateInnoGotchiDto createInnoGotchiDto,
        CancellationToken cancellationToken = new())
    {
        var componentsToCreate = createInnoGotchiDto.Components;
        var components = await GetExistingComponents(componentsToCreate, cancellationToken);
        var farm = await GetProfileFarm(profileId, cancellationToken);
        var innoGotchiComponents = CreateModelComponents(componentsToCreate, components);
        var now = _timeService.Now();

        return new InnoGotchiModel
        {
            HungerLevel = HungerLevel.Normal,
            LastDrinkTime = now,
            LastFeedTime = now,
            Name = createInnoGotchiDto.Name,
            ThirstyLevel = ThirstyLevel.Normal,
            Components = innoGotchiComponents,
            Farm = farm,
            Age = 0,
            AgeUpdatedAt = now,
            HappinessDaysCount = 0,
            LiveSince = now
        };
    }

    private Task<IList<InnoGotchiComponent>> GetExistingComponents(
        List<InnoGotchiModelComponentDto> componentsToCreate,
        CancellationToken cancellationToken = new())
    {
        var urls = componentsToCreate.Select(x => x.ImageUrl).ToList();
        var componentsRepository = _unitOfWork.GetRepository<InnoGotchiComponent>();
        return componentsRepository.GetAllAsync(predicate: x => urls.Contains(x.ImageUrl),
            cancellationToken: cancellationToken);
    }

    private Task<Farm> GetProfileFarm(Guid profileId, CancellationToken cancellationToken = new())
    {
        var farmRepository = _unitOfWork.GetRepository<Farm>();
        return farmRepository.GetFirstOrDefaultAsync(x => x.Owner.Id == profileId,
            cancellationToken: cancellationToken)!;
    }

    private static List<InnoGotchiModelComponent> CreateModelComponents(
        List<InnoGotchiModelComponentDto> dtos,
        IEnumerable<InnoGotchiComponent> existingComponents) =>
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