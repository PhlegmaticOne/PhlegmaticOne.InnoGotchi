using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.OperationResults;

namespace PhlegmaticOne.InnoGotchi.Domain.Providers.Readable;

public interface IReadableAvatarProvider
{
    Task<OperationResult<Avatar>> GetAvatarAsync(Guid profileId);
}