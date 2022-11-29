using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared.Farms;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IFarmManager
{
    Task<OperationResult<DetailedFarmDto>> GetWithPetsAsync(Guid profileId);
    Task<OperationResult<DetailedFarmDto>> GetCollaboratedFarmWithPetsAsync(IdentityModel<Guid> profileIdModel);
    Task<OperationResult<IList<PreviewFarmDto>>> GetCollaboratedAsync(Guid profileId);
    Task<OperationResult<bool>> IsExistsForProfileAsync(Guid profileId);
    Task<OperationResult> CreateAsync(IdentityModel<CreateFarmDto> createFarmIdentityModel);
}