using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableProfileProvider
{
    Task<OperationResult<UserProfile>> GetExistingOrDefaultAsync(string email, string password);
    Task<OperationResult<UserProfile>> GetExistingOrDefaultAsync(Guid profileId);
}