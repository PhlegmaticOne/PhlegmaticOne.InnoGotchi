using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface ICollaborationManager
{
    Task<OperationResult> AddCollaboratorAsync(IdentityModel<CreateCollaborationDto> profileIdentityModel);
}