using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableInnoGotchiesProvider
{
    Task<OperationResult<InnoGotchiModel>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task<OperationResult> DrinkAsync(IdentityModel<Guid> petIdModel);
    Task<OperationResult> FeedAsync(IdentityModel<Guid> petIdModel);
    Task<OperationResult> SynchronizeSignsAsync(IdentityModel<Guid> petIdModel);
}