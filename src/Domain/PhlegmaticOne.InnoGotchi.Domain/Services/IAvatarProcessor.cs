using PhlegmaticOne.InnoGotchi.Domain.Models;

namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface IAvatarProcessor
{
    Task<Avatar> ProcessAvatarAsync(Avatar? avatar, CancellationToken cancellationToken = new());
    Task<byte[]> ProcessAvatarDataAsync(byte[]? avatarData, CancellationToken cancellationToken = new());
}