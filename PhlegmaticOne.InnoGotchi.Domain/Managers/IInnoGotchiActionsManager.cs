using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared.InnoGotchies;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IInnoGotchiActionsManager
{
    Task<OperationResult> UpdateAsync(IdentityModel<UpdateInnoGotchiDto> updatePetModel);
}