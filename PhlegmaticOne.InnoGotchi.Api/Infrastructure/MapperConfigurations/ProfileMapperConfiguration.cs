using AutoMapper;
using PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperResolvers;
using PhlegmaticOne.InnoGotchi.Data.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;

namespace PhlegmaticOne.InnoGotchi.Api.Infrastructure.MapperConfigurations;

public class ProfileMapperConfiguration : Profile
{
    public ProfileMapperConfiguration()
    {
        CreateMap<UserProfile, AuthorizedProfileDto>()
            .ForMember(x => x.JwtToken, o => o.MapFrom<ProfileDtoJwtTokenPropertyResolver>())
            .ForMember(x => x.Email, x => x.MapFrom(y => y.User.Email));

        CreateMap<UserProfile, DetailedProfileDto>()
            .ForMember(x => x.AvatarData, o => o.MapFrom<ProfileAvatarPropertyResolver>())
            .ForMember(x => x.Email, o => o.MapFrom(y => y.User.Email));
    }
}