using AutoMapper;
using PhlegmaticOne.InnoGotchi.Domain.Models;
using PhlegmaticOne.InnoGotchi.Shared.Users;

namespace PhlegmaticOne.InnoGotchi.Services.Infrastructure.MapperConfigurations;

public class ProfilesMapperConfiguration : Profile
{
    public ProfilesMapperConfiguration()
    {
        CreateMap<UserProfile, SearchProfileDto>()
            .ForMember(x => x.Email, o => o.MapFrom(x => x.User.Email));
    }
}