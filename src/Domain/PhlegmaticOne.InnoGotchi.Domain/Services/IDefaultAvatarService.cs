namespace PhlegmaticOne.InnoGotchi.Domain.Services;

public interface IDefaultAvatarService
{
    Task<byte[]> GetDefaultAvatarDataAsync(CancellationToken cancellationToken = new());
}