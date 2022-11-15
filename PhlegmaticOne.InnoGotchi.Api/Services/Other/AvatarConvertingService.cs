using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Data.Models;

namespace PhlegmaticOne.InnoGotchi.Api.Services.Other;

public class AvatarConvertingService : IAvatarConvertingService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const string AvatarsDirectoryName = "Avatars";
    public AvatarConvertingService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }
    public byte[] ConvertAvatar(Avatar? avatar)
    {
        if (avatar?.AvatarData is not null && avatar.AvatarData.Any())
        {
            return avatar.AvatarData;
        }

        var noAvatarFile = WwwRootHelper.GetAllFiles(_webHostEnvironment, AvatarsDirectoryName).First();
        return File.ReadAllBytes(noAvatarFile.FullName);
    }
}