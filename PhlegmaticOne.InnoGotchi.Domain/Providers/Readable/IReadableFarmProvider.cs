using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableFarmProvider
{
    Task<Farm> GetFarmAsync(Guid profileId);
    Task<Farm> GetFarmWithProfileAsync(Guid profileId);
    Task<int> GetPetsCountInFarmAsync(Guid farmId);
}