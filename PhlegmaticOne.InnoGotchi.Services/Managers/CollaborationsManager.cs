using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Domain.Managers;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.InnoGotchi.Shared.Collaborations;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Managers;

public class CollaborationsManager : ICollaborationManager
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWritableCollaborationsProvider _writableCollaborationsProvider;

    public CollaborationsManager(IUnitOfWork unitOfWork,
        IWritableCollaborationsProvider writableCollaborationsProvider)
    {
        _unitOfWork = unitOfWork;
        _writableCollaborationsProvider = writableCollaborationsProvider;
    }

    public async Task<OperationResult<CollaborationDto>> AddCollaboratorAsync(IdentityModel<IdDto> profileIdentityModel)
    {
        var result = await _writableCollaborationsProvider
            .AddCollaboration(profileIdentityModel.ProfileId, profileIdentityModel.Entity.Id);

        await _unitOfWork.SaveChangesAsync();

        var collaboration = result.Result!;
        var collaborationDto = new CollaborationDto
        {
            FarmName = collaboration.Farm.Name,
        };

        return OperationResult.FromSuccess(collaborationDto);
    }
}