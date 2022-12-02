using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableCollaborationsProvider
{
    Task<Collaboration> AddCollaboration(Guid fromProfileId, Guid toProfileId,
        CancellationToken cancellationToken = new());
}