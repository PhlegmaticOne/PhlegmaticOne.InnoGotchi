using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Models.Enums;
using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;
using System.Linq.Expressions;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Domain.Services;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableInnoGotchiProvider : IWritableInnoGotchiesProvider
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IInnoGotchiSignsUpdateService _innoGotchiSignsUpdateService;

    public WritableInnoGotchiProvider(IUnitOfWork unitOfWork, IInnoGotchiSignsUpdateService innoGotchiSignsUpdateService)
    {
        _unitOfWork = unitOfWork;
        _innoGotchiSignsUpdateService = innoGotchiSignsUpdateService;
    }

    public async Task<OperationResult<InnoGotchiModel>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto)
    {
        var created = await CreateInnoGotchi(createInnoGotchiDto);
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
        var createdInnoGotchi = await repository.CreateAsync(created);
        return OperationResult.FromSuccess(createdInnoGotchi);
    }

    public Task<OperationResult> DrinkAsync(IdentityModel<Guid> petIdModel) =>
        ProcessPetUpdating(petIdModel, pet =>
        {
            pet.ThirstyLevel = ThirstyLevel.Full;
            pet.LastDrinkTime = DateTime.Now;
        });

    public Task<OperationResult> FeedAsync(IdentityModel<Guid> petIdModel) =>
        ProcessPetUpdating(petIdModel, pet =>
        {
            pet.ThirstyLevel = ThirstyLevel.Full;
            pet.LastDrinkTime = DateTime.Now;
        });

    public Task<OperationResult> SynchronizeSignsAsync(IdentityModel<Guid> petIdModel) =>
        ProcessPetUpdating(petIdModel, pet =>
        {
            var now = DateTime.Now;
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
        });

    private async Task<OperationResult> ProcessPetUpdating(IdentityModel<Guid> petIdModel, Action<InnoGotchiModel> updateAction)
    {
        var repository = _unitOfWork.GetDataRepository<InnoGotchiModel>();
        var pet = await repository.GetByIdOrDefaultAsync(petIdModel.Entity,
            predicate: WhereProfileIdIs(petIdModel.ProfileId));

        if (pet is null)
        {
            return OperationResult.FromFail($"User hasn't pet with id {petIdModel}");
        }

        if (_innoGotchiSignsUpdateService.IsDeadNow(pet.HungerLevel, pet.ThirstyLevel, pet.Age))
        {
            return OperationResult.FromFail($"You can't update pet with id {petIdModel}, because it is dead");
        }

        var updated = await repository.UpdateAsync(pet, updateAction);
        return OperationResult.FromSuccess(updated);
    }

    private async Task<InnoGotchiModel> CreateInnoGotchi(IdentityModel<CreateInnoGotchiDto> from)
    {
        var entity = from.Entity;
        var componentsToCreate = entity.Components;
        var components = await GetExistingComponents(componentsToCreate);
        var farm = await GetProfileFarm(from.ProfileId);
        var innoGotchiComponents = CreateModelComponents(componentsToCreate, components);
        var now = DateTime.Now;

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
        var componentsRepository = _unitOfWork.GetDataRepository<InnoGotchiComponent>();
        return componentsRepository.GetAllAsync(predicate: x => urls.Contains(x.ImageUrl));
    }

    private Task<Farm> GetProfileFarm(Guid profileId)
    {
        var farmRepository = _unitOfWork.GetDataRepository<Farm>();
        return farmRepository.GetFirstOrDefaultAsync(x => x.OwnerId == profileId)!;
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

    private static Expression<Func<InnoGotchiModel, bool>> WhereProfileIdIs(Guid profileId) =>
        x => x.Farm.OwnerId == profileId;
}