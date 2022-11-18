using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableProfilesProvider
{
    Task<OperationResult<UserProfile>> CreateAsync(RegisterProfileDto registerProfileDto);
    Task<OperationResult<UserProfile>> UpdateAsync(UpdateProfileDto updateProfileDto);
}