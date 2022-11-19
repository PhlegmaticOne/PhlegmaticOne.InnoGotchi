using PhlegmaticOne.InnoGotchi.Domain.Identity;
using PhlegmaticOne.InnoGotchi.Shared;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IInnoGotchiActionsManager
{
    Task<OperationResult> DrinkAsync(IdentityModel<IdDto> petIdModel);
    Task<OperationResult> FeedAsync(IdentityModel<IdDto> petIdModel);
}