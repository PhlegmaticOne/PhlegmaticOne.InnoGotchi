using PhlegmaticOne.InnoGotchi.Shared.Components;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IInnoGotchiComponentsManager
{
    Task<OperationResult<InnoGotchiComponentCollectionDto>> GetAllComponentsAsync();
}