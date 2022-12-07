using PhlegmaticOne.InnoGotchi.Domain.Exceptions;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;
using PhlegmaticOne.InnoGotchi.Shared.ErrorMessages;
using PhlegmaticOne.UnitOfWork.Interfaces;

namespace PhlegmaticOne.InnoGotchi.Services.Providers.Writable;

public class WritableCollaborationsProvider : IWritableCollaborationsProvider
{
    private readonly IUnitOfWork _unitOfWork;

    public WritableCollaborationsProvider(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Collaboration> CreateCollaborationAsync(Guid fromProfileId, Guid toProfileId,
        CancellationToken cancellationToken = new())
    {
        var farm = await GetFarm(fromProfileId, cancellationToken);
        if (farm is null)
        {
            throw new DomainException(AppErrorMessages.FarmDoesNotExistMessage);
        }

        var profile = await GetProfile(toProfileId, cancellationToken);
        if (profile is null)
        {
            throw new DomainException(AppErrorMessages.ProfileDoesNotExistMessage);
        }

        return await _unitOfWork.GetRepository<Collaboration>().CreateAsync(Collaboration(profile, farm), cancellationToken);
    }

    private static Collaboration Collaboration(UserProfile userProfile, Farm farm) =>
        new()
        {
            Collaborator = userProfile,
            Farm = farm
        };

    private Task<Farm?> GetFarm(Guid profileId, CancellationToken cancellationToken = new()) =>
        _unitOfWork.GetRepository<Farm>().GetFirstOrDefaultAsync(
            p => p.Owner.Id == profileId,
            cancellationToken: cancellationToken);

    private Task<UserProfile?> GetProfile(Guid profileId, CancellationToken cancellationToken = new()) =>
        _unitOfWork.GetRepository<UserProfile>()
            .GetByIdOrDefaultAsync(profileId, cancellationToken: cancellationToken);
}