using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IProfileAuthorizedActionsManager
{
    Task<OperationResult<AuthorizedProfileDto>> UpdateAsync(IdentityModel<UpdateProfileDto> updateProfileDto);
    Task<OperationResult<DetailedProfileDto>> GetDetailedAsync(Guid profileId);
}