using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Managers;

public interface IProfileAnonymousActionsManager
{
    Task<OperationResult<AuthorizedProfileDto>> RegisterAsync(RegisterProfileDto registerProfileDto);
    Task<OperationResult<AuthorizedProfileDto>> LoginAsync(LoginDto loginDto);
}