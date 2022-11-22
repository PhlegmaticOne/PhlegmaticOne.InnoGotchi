using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface ICollaborationManager
{
    Task<OperationResult<CollaborationDto>> AddCollaboratorAsync(IdentityModel<IdDto> profileIdentityModel);
}