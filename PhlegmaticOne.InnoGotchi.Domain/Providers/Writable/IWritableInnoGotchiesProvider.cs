using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableInnoGotchiesProvider
{
    Task<InnoGotchiModel> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task DrinkAsync(Guid petId);
    Task FeedAsync(Guid petId);
    Task SynchronizeSignsAsync(Guid petId);
    Task SynchronizeSignsForAllInFarmAsync(Guid farmId);
}