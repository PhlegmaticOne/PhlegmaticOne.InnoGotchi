using Microsoft.EntityFrameworkCore;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableCollaborationsProvider : IWritableCollaborationsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public WritableCollaborationsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Collaboration> AddCollaboration(Guid fromProfileId, Guid toProfileId, 
        CancellationToken cancellationToken = new())
    {
        var farm = await GetFarm(fromProfileId, cancellationToken);
        var collaboration = await CreateCollaboration(toProfileId, farm, cancellationToken);
        farm.Collaborations.Add(collaboration);
        return collaboration;
    }

    private async Task<Collaboration> CreateCollaboration(Guid profileId, Farm farm, CancellationToken cancellationToken = new()) =>
        new()
        {
            Collaborator = await GetProfile(profileId, cancellationToken),
            Farm = farm
        };

    private Task<Farm> GetFarm(Guid profileId, CancellationToken cancellationToken = new()) =>
        _unitOfWork.GetRepository<Farm>().GetFirstOrDefaultAsync(
            predicate: p => p.Owner.Id == profileId,
            cancellationToken: cancellationToken)!;
    private Task<UserProfile> GetProfile(Guid profileId, CancellationToken cancellationToken = new()) =>
        _unitOfWork.GetRepository<UserProfile>()
            .GetByIdOrDefaultAsync(profileId, include: i => i.Include(x => x.User),
                cancellationToken: cancellationToken)!;
}