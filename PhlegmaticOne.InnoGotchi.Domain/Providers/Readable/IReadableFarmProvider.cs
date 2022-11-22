using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableFarmProvider
{
    Task<OperationResult<Farm>> GetFarmAsync(Guid profileId);
    Task<OperationResult<Farm>> GetFarmWithProfileAsync(Guid profileId);
    Task<OperationResult<int>> GetPetsCountInFarmAsync(Guid farmId);
}