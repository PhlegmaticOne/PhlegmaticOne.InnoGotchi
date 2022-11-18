using PhlegmaticOne.OperationResults;
using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableInnoGotchiComponentsProvider
{
    Task<OperationResult<IList<InnoGotchiComponent>>> GetAllComponentsAsync();
}