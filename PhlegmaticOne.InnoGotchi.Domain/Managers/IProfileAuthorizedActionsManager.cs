using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IProfileAuthorizedActionsManager
{
    Task<OperationResult<AuthorizedProfileDto>> UpdateAsync(UpdateProfileDto updateProfileDto);
    Task<OperationResult<DetailedProfileDto>> GetDetailedAsync(Guid profileId);
}