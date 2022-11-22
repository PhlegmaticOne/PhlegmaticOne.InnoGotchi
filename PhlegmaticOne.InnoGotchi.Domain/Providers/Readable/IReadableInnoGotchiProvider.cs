using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableInnoGotchiProvider
{
    Task<OperationResult<InnoGotchiModel>> GetDetailedAsync(Guid petId, Guid profileId);
    Task<OperationResult<IList<InnoGotchiModel>>> GetAllDetailedAsync(Guid farmId);
    Task<OperationResult<IList<InnoGotchiModel>>> GetAllAsync(Guid farmId);
}