using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Farms;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableFarmProvider
{
    Task<Farm> CreateAsync(Guid profileId, CreateFarmDto createFarmDto,
        CancellationToken cancellationToken = new());
}