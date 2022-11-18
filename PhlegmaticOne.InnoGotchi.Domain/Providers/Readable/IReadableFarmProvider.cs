using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableFarmProvider
{
    Task<OperationResult<Farm>> GetFarmAsync(Guid profileId);
}