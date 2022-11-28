using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableAvatarProvider
{
    Task<Avatar> GetAvatarAsync(Guid profileId);
}