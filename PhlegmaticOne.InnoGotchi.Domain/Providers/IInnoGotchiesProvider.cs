using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers;

public interface IInnoGotchiesProvider
{
    Task<OperationResult<InnoGotchiModel>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task<OperationResult<InnoGotchiModel>> GetDetailedAsync(IdentityModel<Guid> petIdModel);
}