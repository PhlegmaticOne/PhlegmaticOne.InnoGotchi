using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableProfileProvider
{
    Task<UserProfile?> GetExistingOrDefaultAsync(string email, string password);
    Task<UserProfile?> GetExistingOrDefaultAsync(Guid profileId);
}