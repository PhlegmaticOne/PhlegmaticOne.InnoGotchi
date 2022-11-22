using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableCollaborationsProvider
{
    Task<OperationResult<Collaboration>> AddCollaboration(Guid fromProfileId, Guid toProfileId);
}