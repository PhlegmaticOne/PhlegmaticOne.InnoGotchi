using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;

namespace PhlegmaticOne.InnoGotchi.Api.Services;

public class DefaultAvatarService : IDefaultAvatarService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const string AvatarsDirectoryName = "Avatars";
    public DefaultAvatarService(IWebHostEnvironment webHostEnvironment) => 
        _webHostEnvironment = webHostEnvironment;

    public Task<byte[]> GetDefaultAvatarDataAsync()
    {
        var noAvatarFile = WwwRootHelper.GetAllFiles(_webHostEnvironment, AvatarsDirectoryName).First();
        return File.ReadAllBytesAsync(noAvatarFile.FullName);
    }
}