using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Services.Other;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;

public class ProfileAvatarPropertyResolver : IValueResolver<UserProfile, DetailedProfileDto, byte[]>
{
    private readonly IAvatarConvertingService _avatarConvertingService;
    public ProfileAvatarPropertyResolver(IAvatarConvertingService avatarConvertingService) => 
        _avatarConvertingService = avatarConvertingService;

    public byte[] Resolve(UserProfile source, DetailedProfileDto destination, byte[] destMember, ResolutionContext context) => 
        _avatarConvertingService.ConvertAvatar(source.Avatar);
}