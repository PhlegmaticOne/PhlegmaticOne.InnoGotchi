using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;
using PhlegmaticOne.OperationResults;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Readable;

public class ReadableCollaborationsProvider : IReadableCollaborationsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public ReadableCollaborationsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<OperationResult<IList<UserProfile>>> GetCollaboratedUsersAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<Farm>();
        var result = await repository.GetFirstOrDefaultAsync(
            predicate: p => p.OwnerId == profileId,
            include: i => i.Include(x => x.Collaborations).ThenInclude(x => x.Collaborator));

        IList<UserProfile> collaborators = result!.Collaborations.Select(x => x.Collaborator).ToList();
        return OperationResult.FromSuccess(collaborators);
    }
}