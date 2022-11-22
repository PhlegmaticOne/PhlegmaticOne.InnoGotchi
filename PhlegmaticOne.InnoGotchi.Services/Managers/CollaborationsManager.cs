using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class CollaborationsManager : ICollaborationManager
{
    public Task<OperationResult<CollaborationDto>> AddCollaboratorAsync(IdentityModel<IdDto> profileIdentityModel)
    {
        throw new NotImplementedException();
    }
}