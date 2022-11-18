using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared.Constructor;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IInnoGotchiManager
{
    Task<OperationResult<DetailedInnoGotchiDto>> CreateAsync(IdentityModel<CreateInnoGotchiDto> createInnoGotchiDto);
    Task<OperationResult<DetailedInnoGotchiDto>> GetDetailedAsync(IdentityModel<Guid> petIdModel);
}