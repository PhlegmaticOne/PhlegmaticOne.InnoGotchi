using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;
using PhlegmaticOne.InnoGotchi.Shared.Dtos.Constructor;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Mapping;

public class InnoGotchiVerifyingService : VerifyingServiceBase<ProfileInnoGotchiModel, InnoGotchiModel>
{
    public InnoGotchiVerifyingService(IValidator<ProfileInnoGotchiModel> fluentValidator, IDataService dataService) : 
        base(fluentValidator, dataService) { }

    public override async Task<InnoGotchiModel> MapAsync(ProfileInnoGotchiModel from)
    {
        var components = await GetExistingComponents(from.Components);
        var farm = await GetProfileFarm(from.ProfileId);
        var innoGotchiComponents = CreateModelComponents(from.Components, components);
        var now = DateTime.UtcNow;

        return new InnoGotchiModel
        {
            HungerLevel = HungerLevel.Normal,
            LastDrinkTime = now,
            LastFeedTime = now,
            Name = from.Name,
            ThirstyLevel = ThirstyLevel.Normal,
            Components = innoGotchiComponents,
            Farm = farm
        };
    }

    private Task<IList<InnoGotchiComponent>> GetExistingComponents(List<ComponentDto> componentsToCreate)
    {
        var urls = componentsToCreate.Select(x => x.ImageUrl).ToList();
        var componentsRepository = DataService.GetDataRepository<InnoGotchiComponent>();
        return componentsRepository.GetAllAsync(predicate: x => urls.Contains(x.ImageUrl));
    }

    private Task<Farm> GetProfileFarm(Guid profileId)
    {
        var farmRepository = DataService.GetDataRepository<Farm>();
        return farmRepository.GetFirstOrDefaultAsync(x => x.OwnerId == profileId)!;
    }

    private static List<InnoGotchiModelComponent> CreateModelComponents(List<ComponentDto> dtos, IEnumerable<InnoGotchiComponent> existingComponents) =>
        dtos.Join(existingComponents, on => on.ImageUrl, on => on.ImageUrl,
            (dto, component) => new InnoGotchiModelComponent
            {
                InnoGotchiComponent = component,
                TranslationX = dto.TranslateX,
                TranslationY = dto.TranslateY,
                ScaleX = dto.ScaleX,
                ScaleY = dto.ScaleY
            }).ToList();
}