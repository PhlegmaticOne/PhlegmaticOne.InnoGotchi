using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableCollaborationsProvider
{
    Task<OperationResult<IList<UserProfile>>> GetCollaboratedUsersAsync(Guid profileId);
    Task<OperationResult<IList<Farm>>> GetCollaboratedFarmsWithUsersAsync(Guid profileId);
}