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

    public async Task<IList<UserProfile>> GetCollaboratedUsersAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<Farm>();
        var result = await repository.GetFirstOrDefaultAsync(
            selector: s => s.Collaborations.Select(x => x.Collaborator),
            predicate: p => p.Owner.Id == profileId,
            include: i => i
                .Include(x => x.Collaborations)
                .ThenInclude(x => x.Collaborator)
                .ThenInclude(x => x.Farm));

        return result!.ToList();
    }

    public async Task<IList<Farm>> GetCollaboratedFarmsWithUsersAsync(Guid profileId)
    {
        var repository = _unitOfWork.GetRepository<UserProfile>();

        var result = await repository.GetByIdOrDefaultAsync(profileId,
            include: i => i
                .Include(x => x.Collaborations)
                .ThenInclude(x => x.Farm)
                .ThenInclude(x => x.Owner)
                .ThenInclude(x => x.User));

        return result!.Collaborations.Select(x => x.Farm).ToList();
    }
}