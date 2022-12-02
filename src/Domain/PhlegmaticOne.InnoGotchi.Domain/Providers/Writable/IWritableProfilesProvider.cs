using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Profiles;
using PhlegmaticOne.InnoGotchi.Shared.Profiles.Anonymous;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Writable;

public interface IWritableProfilesProvider
{
    Task<UserProfile> CreateAsync(RegisterProfileDto registerProfileDto,
        CancellationToken cancellationToken = new());

    Task<UserProfile> UpdateAsync(Guid profileId, UpdateProfileDto updateProfileDto,
        CancellationToken cancellationToken = new());
}