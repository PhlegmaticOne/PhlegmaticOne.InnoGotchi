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
            selector: s => s.Collaborations.Select(x => x.Collaborator),
            predicate: p => p.OwnerId == profileId,
            include: i => i
                .Include(x => x.Collaborations)
                .ThenInclude(x => x.Collaborator)
                .ThenInclude(x => x.Farm));

        IList<UserProfile> resultList = result!.ToList();
        return OperationResult.FromSuccess(resultList);
    }

    public async Task<OperationResult<IList<Farm>>> GetCollaboratedFarmsWithUsersAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetDataRepository<UserProfile>();

        var result = await repository.GetByIdOrDefaultAsync(profileId,
            include: i => i
                .Include(x => x.Collaborations)
                .ThenInclude(x => x.Farm)
                .ThenInclude(x => x.Owner)
                .ThenInclude(x => x.User));

        IList<Farm> resultList = result!.Collaborations.Select(x => x.Farm).ToList();
        return OperationResult.FromSuccess(resultList);
    }
}