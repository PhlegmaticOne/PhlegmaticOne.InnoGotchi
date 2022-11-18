namespace PhlegmaticOne.InnoGotchi.Api.Services;

public interface IDefaultAvatarService
{
    Task<byte[]> GetDefaultAvatarDataAsync();
}