using FluentValidation;
using PhlegmaticOne.DataService.Interfaces;
using PhlegmaticOne.InnoGotchi.Api.Models;
using PhlegmaticOne.InnoGotchi.Api.Services.Mapping.Base;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Data.Models.Enums;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Mapping;

public class InnoGotchiVerifyingService : VerifyingServiceBase<IdentityInnoGotchiModel, InnoGotchiModel>
{
    private readonly IServerAddressProvider _serviceAddressProvider;

    public InnoGotchiVerifyingService(IValidator<IdentityInnoGotchiModel> fluentValidator, IDataService dataService, 
        IServerAddressProvider serviceAddressProvider) : 
        base(fluentValidator, dataService)
    {
        _serviceAddressProvider = serviceAddressProvider;
    }

    public override async Task<InnoGotchiModel> MapAsync(IdentityInnoGotchiModel from)
    {
        var processed = ProcessImagePath(from);
        var components = await GetExistingComponents(processed.Components);
        var farm = await GetProfileFarm(processed.ProfileId);
        var innoGotchiComponents = CreateModelComponents(processed.Components, components);
        var now = DateTime.Now;

        return new InnoGotchiModel
        {
            HungerLevel = HungerLevel.Normal,
            LastDrinkTime = now,
            LastFeedTime = now,
            Name = from.Name,
            ThirstyLevel = ThirstyLevel.Normal,
            Components = innoGotchiComponents,
            Farm = farm,
            Age = 0            
        };
    }

    private IdentityInnoGotchiModel ProcessImagePath(IdentityInnoGotchiModel profileInnoGotchiModel)
    {
        var serverAddress = _serviceAddressProvider.ServerAddress;
        var serverAddressLength = serverAddress.Length;
        profileInnoGotchiModel.Components.ForEach(component =>
        {
            var imageUrl = component.ImageUrl;
            var imageUrlLength = imageUrl.Length;
            component.ImageUrl = imageUrl.Substring(serverAddressLength + 1, imageUrlLength - serverAddressLength - 1);
        });
        return profileInnoGotchiModel;
    }

    private Task<IList<InnoGotchiComponent>> GetExistingComponents(List<InnoGotchiComponentDto> componentsToCreate)
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

    private static List<InnoGotchiModelComponent> CreateModelComponents(List<InnoGotchiComponentDto> dtos, IEnumerable<InnoGotchiComponent> existingComponents) =>
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