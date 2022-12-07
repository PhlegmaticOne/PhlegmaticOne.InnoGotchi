using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableInnoGotchiProvider
{
    Task<InnoGotchiModel?> GetDetailedAsync(Guid petId, CancellationToken cancellationToken = new());
}