using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.Helpers;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;

public class ProfileAvatarPropertyResolver : IValueResolver<UserProfile, DetailedProfileDto, byte[]>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private const string AvatarsDirectoryName = "Resources\\Avatars";

    public ProfileAvatarPropertyResolver(IWebHostEnvironment webHostEnvironment) => 
        _webHostEnvironment = webHostEnvironment;

    public byte[] Resolve(UserProfile source, DetailedProfileDto destination, byte[] destMember, ResolutionContext context)
    {
        if (source.Avatar is not null)
        {
            return source.Avatar.AvatarData;
        }

        var noAvatarFile = WwwRootHelper.GetAllFiles(_webHostEnvironment, AvatarsDirectoryName).First();
        return File.ReadAllBytes(noAvatarFile.FullName);
    }
}