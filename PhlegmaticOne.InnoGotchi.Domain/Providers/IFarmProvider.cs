using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers;

public interface IFarmProvider
{
    Task<OperationResult<Farm>> CreateAsync(IdentityModel<CreateFarmDto> createFarmDto);
    Task<OperationResult<Farm>> GetWithPetsAsync(Guid profileId);
}