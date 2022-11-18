using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers;

public interface IInnoGotchiComponentsProvider
{
    Task<OperationResult<IList<InnoGotchiComponent>>> GetAllAsync();
}