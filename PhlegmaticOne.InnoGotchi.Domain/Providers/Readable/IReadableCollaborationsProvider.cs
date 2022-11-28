using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableCollaborationsProvider
{
    Task<IList<UserProfile>> GetCollaboratedUsersAsync(Guid profileId);
    Task<IList<Farm>> GetCollaboratedFarmsWithUsersAsync(Guid profileId);
}