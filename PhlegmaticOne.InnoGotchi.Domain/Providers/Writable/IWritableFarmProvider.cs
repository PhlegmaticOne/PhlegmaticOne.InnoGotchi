using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableFarmProvider
{
    Task<OperationResult<Farm>> CreateAsync(IdentityModel<CreateFarmDto> createFarmDto);
}