using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableInnoGotchiesProvider
{
    Task<InnoGotchiModel> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task DrinkAsync(IdentityModel<IdDto> petIdModel);
    Task FeedAsync(IdentityModel<IdDto> petIdModel);
    Task SynchronizeSignsAsync(IdentityModel<IdDto> petIdModel);
    Task SynchronizeSignsAsync(Guid farmId);
}