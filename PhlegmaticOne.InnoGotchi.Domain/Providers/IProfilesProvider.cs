using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers;

public interface IProfilesProvider
{
    Task<OperationResult<UserProfile>> CreateAsync(RegisterProfileDto registerProfileDto);
    Task<OperationResult<UserProfile>> GetExistingOrDefaultAsync(string email, string password);
    Task<OperationResult<UserProfile>> GetExistingOrDefaultAsync(Guid profileId);
    Task<OperationResult<UserProfile>> UpdateAsync(UpdateProfileDto updateProfileDto);
}